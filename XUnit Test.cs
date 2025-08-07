using ActualLab.CommandR;
using ActualLab.Fusion;
using ActualLab.Fusion.EntityFramework;
using ActualLab.Fusion.EntityFramework.Operations;
using ActualLab.Rpc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Stl.Fusion;
using System.Data;

namespace CheckPointBackendTest.ServiceTests;

public class BranchServiceTest
{
    private readonly ServiceProvider _serviceProvider;
    private readonly ICommander _commander;
    private IBranchService _branchService;

    public BranchServiceTest()
    {
        var services = new ServiceCollection();

        var fusion = services.AddFusion(RpcServiceMode.Server, true);
        fusion.AddService<IBranchService, BranchService>();

        services.AddTransient(_ => new DbOperationScope<AppDbContext>.Options
        {
            IsolationLevel = IsolationLevel.Unspecified
        });

        services.AddDbContextServices<AppDbContext>(db => {
            db.AddOperations();
            db.Services.AddDbContextFactory<AppDbContext>((sp, options) =>
            {
                options
                    .UseInMemoryDatabase("TestBranchDb")
                    .ConfigureWarnings(w =>
                        w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        });

        _serviceProvider = services.BuildServiceProvider();
        _commander = _serviceProvider.GetRequiredService<ICommander>();
        _branchService = _serviceProvider.GetRequiredService<IBranchService>();
    }

    [Fact]
    public async Task Create_ValidBranch_CreatesSuccessfully()
    {
        await _commander.Call(new CreateBranchCommand(new() { Name = "New Branch" }));

        var result = await _branchService.GetAll(new TableOptions { Page = 1, PageSize = 10 });

        Assert.Contains(result.Items, b => b.Name == "New Branch");
    }

    [Fact]
    public async Task Get_WithValidId_ReturnsBranch()
    {
        await _commander.Call(new CreateBranchCommand(new() { Name = "Test Branch" }));

        var branches = await _branchService.GetAll(new TableOptions { Page = 1, PageSize = 10 });
        var branch = branches.Items.First();

        var result = await _branchService.Get(branch.Id);

        Assert.NotNull(result);
        Assert.Equal("Test Branch", result.Name);
    }

    [Fact]
    public async Task Get_NonExistingId_ThrowsNotFoundException()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => _branchService.Get(9999));
    }

    [Fact]
    public async Task Update_ExistingBranch_UpdatesSuccessfully()
    {
        await _commander.Call(new CreateBranchCommand(new() { Name = "Test Branch" }));

        var branch = (await _branchService.GetAll(new TableOptions { Page = 1, PageSize = 10 }))
            .Items.First();

        await _commander.Call(new UpdateBranchCommand(new() { Id = branch.Id, Name = "Updated Name" }));

        var updated = await _branchService.Get(branch.Id);
        Assert.Equal("Updated Name", updated.Name);
    }

    [Fact]
    public async Task Delete_ExistingBranch_DeletesSuccessfully()
    {
        await _commander.Call(new CreateBranchCommand(new() { Name = "Test Branch" }));

        var branch = (await _branchService.GetAll(new TableOptions { Page = 1, PageSize = 10 }))
            .Items.First();

        await _commander.Call(new DeleteBranchCommand(branch.Id));

        var result = await _branchService.GetAll(new TableOptions { Page = 1, PageSize = 10 });
        Assert.DoesNotContain(result.Items, b => b.Id == branch.Id);
    }

    [Fact]
    public async Task GetAll_WithTableOptions_ReturnsCorrectResults()
    {
        await _commander.Call(new CreateBranchCommand(new() { Name = "Paged Branch" }));

        var result = await _branchService.GetAll(new TableOptions { Page = 1, PageSize = 10 });

        Assert.NotNull(result);
        Assert.True(result.Items.Any());
    }

    [Fact]
    public async Task GetAll_ShouldCacheResult()
    {            
        await _commander.Call   (new CreateBranchCommand(new() { Name = "Cached Branch" }));

        var result1 = await _branchService.GetAll(new TableOptions { Page = 1, PageSize = 10 });
        var result2 = await _branchService.GetAll(new TableOptions { Page = 1, PageSize = 10 });

        Assert.Equal(result1.TotalItems, result2.TotalItems);
    }

    [Fact]
    public async Task Create_ShouldInvalidateCache()
    {
        await _commander.Call(new CreateBranchCommand(new() { Name = "Initial Branch" }));

        var result1 = await _branchService.GetAll(new TableOptions { Page = 1, PageSize = 10 });

        await _commander.Call(new CreateBranchCommand(new() { Name = "New Branch" }));

        var result2 = await _branchService.GetAll(new TableOptions { Page = 1, PageSize = 10 });

        Assert.Equal(result1.TotalItems + 1, result2.TotalItems);
    }

    [Fact]
    public async Task Update_ShouldInvalidateCache()
    {
        await _commander.Call(new CreateBranchCommand(new() { Name = "ToUpdate" }));

        var branch = (await _branchService.GetAll(new TableOptions { Page = 1, PageSize = 10 }))
            .Items.First();

        await _commander.Call(new UpdateBranchCommand(new() { Id = branch.Id, Name = "UpdatedName" }));

        var updated = await _branchService.Get(branch.Id);
        Assert.Equal("UpdatedName", updated.Name);
    }

    [Fact]
    public async Task Delete_ShouldInvalidateCache()
    {
        await _commander.Call(new CreateBranchCommand(new() { Name = "ToDelete" }));

        var branch = (await _branchService.GetAll(new TableOptions { Page = 1, PageSize = 10 }))
            .Items.First();

        var countBefore = (await _branchService.GetAll(new TableOptions { Page = 1, PageSize = 10 }))
            .TotalItems;

        await _commander.Call(new DeleteBranchCommand(branch.Id));

        var countAfter = (await _branchService.GetAll(new TableOptions { Page = 1, PageSize = 10 }))
            .TotalItems;

        Assert.Equal(countBefore - 1, countAfter);
    }
}

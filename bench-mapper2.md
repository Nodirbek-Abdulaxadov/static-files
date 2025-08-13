Mana sizga birinchi jadval va umumiy xulosa birlashtirilgan holda:

| Method Pair             | Avg Mean (Old) | Avg Mean (New) | Change (us) | Change (%) | Avg Allocated (Old) | Avg Allocated (New) | Xulosa                                                                      |
| ----------------------- | -------------: | -------------: | ----------: | ---------: | ------------------: | ------------------: | --------------------------------------------------------------------------- |
| **GetAll vs GetAllOld** |       13,943.6 |       13,927.1 |   **-16.5** | **-0.12%** |            90.17 KB |            89.71 KB | Tezlik va xotira deyarli bir xil, yangi kod ozgina tezroq va yengilroq.     |
| **Get vs GetOld**       |          784.6 |          798.5 |   **+13.9** | **+1.77%** |            65.09 KB |            68.20 KB | Yangi versiya o‘rtacha \~14 µs sekinroq va \~3 KB ko‘proq xotira ishlatadi. |

**Umumiy xulosa:**

* **GetAll** — Yangi va eski versiyalar orasida sezilarli farq yo‘q, ba’zan yangi kod biroz tezroq va kamroq xotira ishlatadi.
* **Get** — Yangi versiya biroz sekinroq (\~2%) va ko‘proq xotira talab qiladi.
* **Sort by Name** — Har ikkala versiyada ham eng katta ishlash xarajati shu sort turi bo‘lib qolmoqda (Id yoki CreatedAt dan ancha sekin).

Agar xohlasangiz, men sizga **har bir SortLabel bo‘yicha grafik** chiqarib, qaysi joyda yutib, qaysi joyda yutqazayotganini aniq ko‘rsatishim mumkin.


---

Mana sizga birinchi jadval va umumiy xulosa birlashtirilgan holda:

| Method Pair             | Avg Mean (Old) | Avg Mean (New) | Change (us) | Change (%) | Avg Allocated (Old) | Avg Allocated (New) | Xulosa                                                                      |
| ----------------------- | -------------: | -------------: | ----------: | ---------: | ------------------: | ------------------: | --------------------------------------------------------------------------- |
| **GetAll vs GetAllOld** |       13,943.6 |       13,927.1 |   **-16.5** | **-0.12%** |            90.17 KB |            89.71 KB | Tezlik va xotira deyarli bir xil, yangi kod ozgina tezroq va yengilroq.     |
| **Get vs GetOld**       |          784.6 |          798.5 |   **+13.9** | **+1.77%** |            65.09 KB |            68.20 KB | Yangi versiya o‘rtacha \~14 µs sekinroq va \~3 KB ko‘proq xotira ishlatadi. |

**Umumiy xulosa:**

* **GetAll** — Yangi va eski versiyalar orasida sezilarli farq yo‘q, ba’zan yangi kod biroz tezroq va kamroq xotira ishlatadi.
* **Get** — Yangi versiya biroz sekinroq (\~2%) va ko‘proq xotira talab qiladi.
* **Sort by Name** — Har ikkala versiyada ham eng katta ishlash xarajati shu sort turi bo‘lib qolmoqda (Id yoki CreatedAt dan ancha sekin).

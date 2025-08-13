## **Qisqa Xulosa**

* **GetOld** eng tez va eng kam xotira ishlatadi — bir dona element olishda eng yaxshi tanlov.
* **Get** biroz sekinroq, lekin barqaror va xotira sarfi ham past.
* **GetAll** — eski versiyaga nisbatan ancha tez va kamroq xotira ishlatadi, ko‘p element olish uchun tavsiya etiladi.
* **GetAllOld** eng sekin va ko‘p xotira ishlatadi, ayniqsa **Name** bo‘yicha saralashda.
* **Name** bo‘yicha sort qilish eng ko‘p vaqt va xotira talab qiladi.
* **PageSize** oshishi **GetAll** va **GetAllOld** ishlashini sezilarli yomonlashtiradi, lekin **Get** va **GetOld** deyarli ta’sirlanmaydi.

---

### **Yig‘ma Jadval**

| **Ko‘rsatkich**           | **GetOld**                | **Get**             | **GetAllOld**                   | **GetAll**                 |
| ------------------------- | ------------------------- | ------------------- | ------------------------------- | -------------------------- |
| **O‘rtacha vaqt (us)**    | \~900–1 000 (**Eng tez**) | \~1 100–1 400       | \~2 300–7 500 (**Eng sekin**)   | \~1 800–5 150 (**Tezroq**) |
| **Xato / StdDev**         | Past                      | Past                | Yuqori                          | O‘rtacha                   |
| **Reyting**               | 1 (**Eng yaxshi**)        | 1 (**Eng yaxshi**)  | 2–6 (**Yomon Name/katta Page**) | 2–4 (**Yaxshiroq**)        |
| **Xotira (KB)**           | \~70 (**Eng past**)       | \~88                | \~203–365 (**Eng yuqori**)      | \~150–221 (**Pastroq**)    |
| **Eng yaxshi holat**      | Bitta element olish       | Bitta element olish | Faqat majbur bo‘lganda          | Ko‘p element olish         |
| **Sort ta’siri**          | Minimal                   | Minimal             | Yuqori (Name)                   | O‘rtacha (Name)            |
| **Page/PageSize ta’siri** | Ta’sirsiz                 | Ta’sirsiz           | Yomonlashadi                    | Yomonlashadi, lekin kamroq |

---

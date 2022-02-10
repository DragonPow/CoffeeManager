# **Coffee Manager**

Project is used for subject Software Testing & Software Technology, UIT.

The **final report** can see at [here](https://docs.google.com/document/d/17x6inNuj88CdlD8b9OlWHklIczktyhBv/edit?usp=sharing&ouid=104498979961164559065&rtpof=true&sd=true), or another important file is [**SRS**](https://drive.google.com/drive/folders/1-JbG8eJxoO6VGR6rnJSwN0nUlTTR9W7Z?usp=sharing).

## **About team**

The project have 5 member:
- [Vũ Ngọc Thạch](https://github.com/vungocthach) - MSSV: 19520266
- [Nguyễn Phạm Duy Bằng](https://github.com/bang2001vl) - MSSV: 19520397
- [Huỳnh Thị Minh Nhực](https://github.com/HuynhThiMinhNhuc) - MSSV: 19521973
- [Đoàn Ngọc Lãm](https://github.com/lamngok1201) - MSSV: 19521737
- [Võ Thị Thủy Tiên](https://github.com/thuytien192) - MSSV: 19520296

__Time start:__ 23/03/2021

__Time predict end:__ 23/5/2021

---
## **Main features**

- Manage product (image, name, price by VND, description)
- Manage category
- Manage table: change status (free, have gest, be damage), CRUD
- Orders (automatic billing): 
  - with **category**, **name** filter
  - by table or bring to home
  - export to PDF
- History: search between 2 days
- Statistic: 
  - after month by **chart** and **table**
  - show detail revenue of each product by month
- Information of shop: easy to change **name**, **phone**, **address**

---
## **About software**
Language: C#

Platform: WPF

Pattern: MVVM

Database: SQLite

- Directory file: `~/MainProject/db-ver2.db`
- Structure: refer at [draw.io](https://drive.google.com/file/d/1bwCPXMyX1HM0h68XuMt7Pf8jIj-hM2iy/view?usp=sharing) (Main tab)

Framework:
- [Entity Framework 6.0](https://www.entityframeworktutorial.net/entityframework6/introduction.aspx) (DB-first)
- [Material Design](https://material.io/)

---
## **Some UI of app**
**Main tab**

![](UI%20image/Main.png)

![](UI%20image/Main_with_table.png)

![](UI%20image/Main_with_cart.png)

**Payment view**

<img src="UI%20image/Bill_payment.png" alt='drawing' height='500'/>

**Product tab**

- Main tab:
  
<img src="UI%20image/Menu.png" alt='drawing'/>

- Interact with product:

<table>
    <td>
        <img src="UI%20image/Add_dish.png" alt='drawing'/>
    </td>
    <td>
        <img src="UI%20image/Edit_dish.png" alt='drawing'/>
    </td>
</table>

- Interact with category:

<p align="center">
    </p><img src="UI%20image/Add_category.png" alt='drawing' height='200'/>
</p>

<table>
    <td>
        <img src="UI%20image/Edit_category.png" alt='drawing' height='300'/>
    </td>
    <td>
        <img src="UI%20image/Edit_category_change_dish.png" alt='drawing' height='300'/>
    </td>
        <td>
        <img src="UI%20image/Edit_category_change_name.png" alt='drawing' height='300'/>
    </td>
</table>

**Table tab**

- Color: Green (free), Red (have gest), Gray (damaged)

<table>
    <td>
        <img src="UI%20image/Table.png" alt='drawing' height='300'/>
    </td>
    <td>
        <img src="UI%20image/Table_button.gif" alt='drawing' height='200'/>
    </td>
</table>

**History tab**

<img src="UI%20image/History.png" alt='History'/>

**Statistic tab**

<img src="UI%20image/Statistic.png" alt='Statistic'/>

<img src="UI%20image/Statistic_detail.png" alt='Statistic_detail'/>

**Information tab**

<img src="UI%20image/Information.png" alt='drawing'/>

**Some another UI**

- Billing PDF

<img src="UI%20image/pdf.png" alt='pdf' height="500"/>

- Messagebox

<img src="UI%20image/messs.png" alt='drawing' height="200"/>

# Testing

We use NUnit to test some method (not all) in project, see [Test Report](https://docs.google.com/spreadsheets/d/1p2PTyvtfbf3tW9qwVqhlZpjp1aXMTPhs/edit?usp=sharing&ouid=104498979961164559065&rtpof=true&sd=true) or [Test Case](https://docs.google.com/spreadsheets/d/1lxxBu1xzD3XMIWXeV6Wzd4Lx3Itiml3x/edit?usp=sharing&ouid=104498979961164559065&rtpof=true&sd=true) for more understand.


Test code can find at `~/NUnitTestProject`.

Techniques:
- Blackbox
- Whitebox
- Tool: [TestComplete](https://smartbear.com/product/testcomplete/overview/), video record of project at [here](https://drive.google.com/drive/folders/1lde6ud6ZBL9bz_ZJvGpM2S_4kDLz22JF?usp=sharing)

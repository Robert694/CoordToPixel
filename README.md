# CoordToPixel
 A C# library for converting game coordinates to image pixel coordinates

The example project converts ArcheAge coordinates.
Given a `Configuration.ini` contents of: 
```ini
[Ipnya Ridge]
ImagePath=https://i.imgur.com/juIa77o.png
GameCoord1=W 04 59 31 N 03 25 21
ImageCoord1=729, 78
GameCoord2=W 08 11 03 N 01 00 57
ImageCoord2=285, 412
GridLineDegreeX=0.25
GridLineDegreeY=0.25

Points[]=W 07 58 36 N 01 03 45
Points[]=W 08 11 03 N 01 00 57
Points[]=W 08 09 59 N 00 55 10
Points[]=W 08 05 32 N 00 56 56
Points[]=W 07 58 33 N 01 06 22
Points[]=W 07 47 57 N 01 16 01
Points[]=W 07 39 11 N 01 17 10
Points[]=W 07 19 42 N 01 32 55
Points[]=W 07 08 43 N 01 40 53
Points[]=W 07 09 59 N 01 48 40
Points[]=W 07 08 17 N 02 03 38
Points[]=W 07 21 58 N 01 58 40
Points[]=W 07 26 28 N 01 57 02
Points[]=W 07 30 04 N 01 43 00
Points[]=W 07 32 09 N 01 24 40
Points[]=W 07 39 15 N 01 17 09
Points[]=W 07 47 22 N 01 27 50
Points[]=W 07 58 30 N 01 15 39
Points[]=W 07 58 36 N 01 03 45
Points[]=W 08 11 03 N 01 00 57
Points[]=W 08 09 59 N 00 55 10
Points[]=W 08 05 32 N 00 56 56
Points[]=W 07 58 33 N 01 06 22
Points[]=W 07 47 57 N 01 16 01
Points[]=W 07 01 03 N 01 47 53
Points[]=W 07 06 11 N 02 16 34
Points[]=W 07 06 11 N 02 16 34

[Marianople City]
ImagePath=https://i.imgur.com/gPgVVZZ.png
GameCoord1=W 10 29 46 S 16 12 18
ImageCoord1=125, 219
GameCoord2=W 09 43 52 S 16 20 51
ImageCoord2=741, 333

Points[]=W 10 29 46 S 16 12 18
Points[]=W 09 43 52 S 16 20 51
Points[]=W 10 07 33 S 16 17 08
```
The resulting images would be placed in the Output directory

<img src="https://i.imgur.com/bdX8t8U.png" alt="133152966527691290_alpha_Ipnya Ridge.png" width="500"/> <img src="https://i.imgur.com/jRaagNK.png" alt="133152966527691290_map_Ipnya Ridge.png" width="500"/>
<img src="https://i.imgur.com/8G9KPNo.png" alt="133152966535403523_alpha_Marianople City.png" width="500"/> <img src="https://i.imgur.com/5PzwGRQ.png" alt="133152966535403523_map_Marianople City.png" width="500"/>

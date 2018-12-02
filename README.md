# GoPro Encoder
This is just a .NET (4.7) console app that will take all Gopro videos added to a directory and:

i. Rename with GPyyyyMMdd_hhmmss.mp4 filename

ii. Encode with x265/HEVC via Handbrake CLI to save space

Make sure the build is targeting x64 as MediaInfo.dll is architecture specific.

Instead of checked-in versions, download third party dependencies from source:

1) Handbrake CLI
https://handbrake.fr/downloads2.php

2) MediaInfo.dll
https://mediaarea.net/en/MediaInfo/Download/Windows

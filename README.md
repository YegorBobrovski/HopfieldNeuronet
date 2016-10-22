# Hopfield_neuronet
Simple WPF implementation of Hopfield's neuronet. Network is learning several encoded images and then tries to recognize them with different noise levels.

## Prerequisites
[.NET Framework 4.5.2](https://www.microsoft.com/en-us/download/details.aspx?id=42643)  

## Installing
[MahApps.Metro](http://mahapps.com/) - UI toolkit for WPF. Can be restored automatically with Package Manager Console
```
Update-Package -Reinstall -ProjectName NeuroNet
```

## Usage
To add custom 10x10 images just modify *srcimgs.txt* file as following:
```
# this line is ignored, use it for comments or image safe deletion  
  
# text after '//' is used as tag and tooltip for image below
// Z  
# '|' for white pixel, any other character for black
_||||||||_  
_||||||||_  
______|||_  
_____|||__  
____|||___  
___|||____  
__|||_____  
_|||______  
_||||||||_  
_||||||||_  

# leave empty row at the end of file
```
Ensure that *srcimgs.txt* is copied to output directory during build  

On GUI select image from left list and see generated output in right list. To generate again just click on preview image.

## Authors
* [**Yegor Bobrovski**](https://github.com/YegorBobrovski) - BSUIR student (graduated in 2017)
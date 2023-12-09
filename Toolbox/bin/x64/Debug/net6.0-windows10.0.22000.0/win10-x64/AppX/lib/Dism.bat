@ECHO OFF

::bat [wim drv] [win盘符] [wim或驱动文件夹路径]
::        1         2             3

if "%1"=="wim" Dism /Apply-Image /ImageFile:"%3" /index:1 /ApplyDir:%2:\ || ECHO.出错了！可能的原因：1.系统包损坏(重新下载) 2.被安全软件阻止(退出安全软件重试) 3.电脑系统Dism组件损坏(更换电脑或重装官方系统) 4.设备连接中断(检查设备连接，更换数据线和插口)。
if "%1"=="drv" Dism /Image:%2:\ /Add-Driver /Driver:%3 /Recurse /ForceUnsigned || ECHO.安装驱动出错！
if "%1"=="drvon" pnputil /add-driver %3 /subdirs /install

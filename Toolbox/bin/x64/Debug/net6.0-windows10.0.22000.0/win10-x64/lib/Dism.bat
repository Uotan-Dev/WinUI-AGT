@ECHO OFF

::bat [wim drv] [win�̷�] [wim�������ļ���·��]
::        1         2             3

if "%1"=="wim" Dism /Apply-Image /ImageFile:"%3" /index:1 /ApplyDir:%2:\ || ECHO.�����ˣ����ܵ�ԭ��1.ϵͳ����(��������) 2.����ȫ�����ֹ(�˳���ȫ�������) 3.����ϵͳDism�����(�������Ի���װ�ٷ�ϵͳ) 4.�豸�����ж�(����豸���ӣ����������ߺͲ��)��
if "%1"=="drv" Dism /Image:%2:\ /Add-Driver /Driver:%3 /Recurse /ForceUnsigned || ECHO.��װ��������
if "%1"=="drvon" pnputil /add-driver %3 /subdirs /install

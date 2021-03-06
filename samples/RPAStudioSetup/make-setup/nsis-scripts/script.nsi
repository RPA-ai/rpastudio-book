; 安装程序初始定义常量
!include "nsProcess.nsh"
!include "WordFunc.nsh"

!define PRODUCT_PUBLISHER ""
!define PRODUCT_WEB_SITE ""
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\RPAStudio.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; 提升安装程序权限(vista,win7,win8)
RequestExecutionLevel admin

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI.nsh"

VIProductVersion "${PRODUCT_VERSION}";版本号
VIAddVersionKey FileDescription "${PRODUCT_NAME}" ;文件描述(标准信息) 
VIAddVersionKey FileVersion "${PRODUCT_VERSION}" ;文件版本(标准信息) 
VIAddVersionKey ProductName "${PRODUCT_NAME}" ;产品名称 
VIAddVersionKey ProductVersion "${PRODUCT_VERSION}" ;产品版本 
VIAddVersionKey CompanyName "${PRODUCT_PUBLISHER}" ;公司名
VIAddVersionKey LegalCopyright "版权所有 © ${PRODUCT_COPYRIGHT_YEAR} " ;合法版权 

; MUI 预定义常量
!define MUI_ABORTWARNING
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Header\orange.bmp"
!define MUI_HEADERIMAGE_UNBITMAP "${NSISDIR}\Contrib\Graphics\Header\orange-uninstall.bmp"
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\orange-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\orange-uninstall.ico"
!define MUI_WELCOMEFINISHPAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Wizard\orange.bmp"
!define MUI_UNWELCOMEFINISHPAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Wizard\orange-uninstall.bmp"

; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 许可协议页面
; !insertmacro MUI_PAGE_LICENSE "${NSISDIR}\license.txt"
; 安装目录选择页面
!insertmacro MUI_PAGE_DIRECTORY
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES
; 安装完成页面
!define MUI_FINISHPAGE_RUN "$INSTDIR\RPAStudio.exe"
!insertmacro MUI_PAGE_FINISH

; 安装卸载过程页面
!insertmacro MUI_UNPAGE_WELCOME
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_UNPAGE_FINISH

; 安装界面包含的语言设置
!insertmacro MUI_LANGUAGE "SimpChinese"

; 安装预释放文件
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI 现代界面定义结束 ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "${SETUP_EXE_NAME}-v${PRODUCT_VERSION}.exe"
InstallDir "$PROGRAMFILES\RPAStudio"
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
ShowInstDetails show
ShowUnInstDetails show
BrandingText " "




Function GetNetFrameworkVersion
;获取.Net Framework版本支持
Push $1
Push $0
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full""Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full""Version"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5""Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5""Version"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0\Setup" "InstallSuccess"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.0\Setup" "Version"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v2.0.50727" "Version"
StrCmp $1 "" +1 +2
StrCpy $1 "2.0.50727.832"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v1.1.4322" "Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v1.1.4322" "Version"
StrCmp $1 "" +1 +2
StrCpy $1 "1.1.4322.573"
StrCmp $0 1 KnowNetFrameworkVersion +1
ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\.NETFramework\policy\v1.0""Install"
ReadRegDWORD $1 HKLM "SOFTWARE\Microsoft\.NETFramework\policy\v1.0""Version"
StrCmp $1 "" +1 +2
StrCpy $1 "1.0.3705.0"
StrCmp $0 1 KnowNetFrameworkVersion +1
StrCpy $1 "not .NetFramework"
KnowNetFrameworkVersion:
Pop $0
Exch $1
FunctionEnd


Function .onInit
  ;关闭进程
CheckProc:
    ${nsProcess::FindProcess} "RPAStudio.exe" $R0
    ${If} $R0 == "0"
        # it's running
        MessageBox MB_OKCANCEL|MB_ICONSTOP "安装程序检测到 ${PRODUCT_NAME} 正在运行。$\r$\n$\r$\n点击 “确定” 强制关闭${PRODUCT_NAME}，继续安装。$\r$\n点击 “取消” 退出安装程序。" IDCANCEL Exit
        ${nsProcess::KillProcess} "RPAStudio.exe" $R0
        Sleep 1000
        Goto CheckProc
    ${Else}
        Goto Done
    ${EndIf}
    Exit:
    Abort
    Done:
	
	
CheckProc2:
    ${nsProcess::FindProcess} "RPARobot.exe" $R0
    ${If} $R0 == "0"
        # it's running
        MessageBox MB_OKCANCEL|MB_ICONSTOP "安装程序检测到 ${ROBOT_NAME} 正在运行。$\r$\n$\r$\n点击 “确定” 强制关闭${ROBOT_NAME}，继续安装。$\r$\n点击 “取消” 退出安装程序。" IDCANCEL Exit2
        ${nsProcess::KillProcess} "RPARobot.exe" $R0
        Sleep 1000
        Goto CheckProc2
    ${Else}
        Goto Done2
    ${EndIf}
    Exit2:
    Abort
    Done2:
FunctionEnd



Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite on
  File /r "${PACKAGE_DIR}\*.*"

  CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME}"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk" "$INSTDIR\RPAStudio.exe"
  CreateShortCut "$DESKTOP\${PRODUCT_NAME}.lnk" "$INSTDIR\RPAStudio.exe"
  
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\${ROBOT_NAME}.lnk" "$INSTDIR\RPARobot.exe"
  CreateShortCut "$DESKTOP\${ROBOT_NAME}.lnk" "$INSTDIR\RPARobot.exe"
SectionEnd

Section -AdditionalIcons
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\卸载${PRODUCT_NAME}.lnk" "$INSTDIR\uninst.exe"
SectionEnd


Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\RPAStudio.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\RPAStudio.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd

Section -CompareDotNetVersion
  DetailPrint "正在检测安装环境..."
  Call GetNetFrameworkVersion
  Pop $R1
  ${VersionCompare} "4.6.01055" "$R1" $R2
  ${If} $R2 == 0
    DetailPrint "当前.Net Framework组件版本($R1)，无需安装组件"
  ${ElseIf} $R2 == 1
    DetailPrint "当前.Net Framework组件版本($R1)过低,需要安装(4.6.1)版本的组件"
  ${ElseIf} $R2 == 2
    DetailPrint "当前.Net Framework组件版本($R1)，无需安装组件"
  ${EndIf}
SectionEnd

Section -InstallDotNET
 Call GetNetFrameworkVersion
 Pop $R1
 ${VersionCompare} "4.6.01055" $R1 $R2
  ${If} $R2 == 1      
       DetailPrint "准备安装.Net Framework 4.6.1组件"
       ExecWait '$INSTDIR\dotnet\NDP461-KB3102436-x86-x64-AllOS-ENU.exe /norestart /passive /showrmui /ChainingPackage RPAStudio' $R1
       ${If} $R1 == 0
           DetailPrint "准备安装.Net Framework 4.6.1组件语言包"
           ExecWait '$INSTDIR\dotnet\NDP461-KB3102436-x86-x64-AllOS-CHS.exe /norestart /passive /showrmui /ChainingPackage RPAStudio' $R1
       ${EndIf}
  ${EndIf}
SectionEnd

Section -DeleteCachedPackages
 DetailPrint "正在清除缓存的依赖包..."
 Delete "$LOCALAPPDATA\RPAStudio\Packages\*.*"
 RMDir /r "$LOCALAPPDATA\RPAStudio\Packages"
SectionEnd


/******************************
*  以下是安装程序的卸载部分  *
******************************/

Section Uninstall
    ;关闭进程
CheckProc:
    ${nsProcess::FindProcess} "RPAStudio.exe" $R0
    ${If} $R0 == "0"
        ${nsProcess::KillProcess} "RPAStudio.exe" $R0
        Sleep 1000
        Goto CheckProc
    ${Else}
        Goto Done
    ${EndIf}
Done:

CheckProc2:
    ${nsProcess::FindProcess} "RPARobot.exe" $R0
    ${If} $R0 == "0"
        ${nsProcess::KillProcess} "RPARobot.exe" $R0
        Sleep 1000
        Goto CheckProc2
    ${Else}
        Goto Done2
    ${EndIf}
Done2:

  Delete "$INSTDIR\*.*"

  Delete "$SMPROGRAMS\${PRODUCT_NAME}\卸载${PRODUCT_NAME}.lnk"
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\${PRODUCT_NAME}.lnk"
  Delete "$DESKTOP\${PRODUCT_NAME}.lnk"
  
  Delete "$SMPROGRAMS\${PRODUCT_NAME}\${ROBOT_NAME}.lnk"
  Delete "$DESKTOP\${ROBOT_NAME}.lnk"

  RMDir "$SMPROGRAMS\${PRODUCT_NAME}"

  RMDir /r "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
SectionEnd


/* 根据 NSIS 脚本编辑规则,所有 Function 区段必须放置在 Section 区段之后编写,以避免安装程序出现未可预知的问题. */

Function un.onInit
FunctionEnd

Function un.onUninstSuccess
FunctionEnd

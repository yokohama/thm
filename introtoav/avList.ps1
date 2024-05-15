<#
WindowsServerで以下のコマンドが使えない場合の代替PowerShell
- wmic /namespace:\\root\securitycenter2 path antivirusproduct
- Get-CimInstance -Namespace root/SecurityCenter2 -ClassName AntivirusProduct
#>

Get-WmiObject -Query "select * from Win32_Process" | 
  Where-Object { 
    $_.Name -like "MsMpEng.exe" -or
    $_.Name -like "AdAwareService.exe" -or
    $_.Name -like "afwServ.exe" -or
    $_.Name -like "avguard.exe" -or
    $_.Name -like "AVGSvc.exe" -or
    $_.Name -like "bdagent.exe" -or
    $_.Name -like "BullGuardCore.exe" -or
    $_.Name -like "ekrn.exe" -or
    $_.Name -like "fshoster32.exe" -or
    $_.Name -like "GDScan.exe" -or
    $_.Name -like "avp.exe" -or
    $_.Name -like "K7CrvSvc.exe" -or
    $_.Name -like "McAPExe.exe" -or
    $_.Name -like "NortonSecurity.exe" -or
    $_.Name -like "PavFnSvr.exe" -or
    $_.Name -like "SavService.exe" -or
    $_.Name -like "EnterpriseService.exe" -or
    $_.Name -like "WRSA.exe" -or
    $_.Name -like "ZAPrivacyService.exe"
  } |
  Select-Object Name

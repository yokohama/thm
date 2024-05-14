import psutil

a = [
    "MsMpEng.exe",
    "AdAwareService.exe",
    "afwServ.exe",
    "avguard.exe",
    "AVGSvc.exe",
    "bdagent.exe",
    "BullGuardCore.exe",
    "ekrn.exe",
    "fshoster32.exe",
    "GDScan.exe",
    "avp.exe",
    "K7CrvSvc.exe",
    "McAPExe.exe",
    "NortonSecurity.exe",
    "PavFnSvr.exe",
    "SavService.exe",
    "EnterpriseService.exe",
    "WRSA.exe",
    "ZAPrivacyService.exe"
]

for proc in psutil.process_iter(['pid', 'name']):
    if proc.info['name'] in a:
      print(f"PID: {proc.info['pid']}, {proc.info['name']}")

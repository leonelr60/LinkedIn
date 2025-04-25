SET TALK OFF
SET ECHO OFF
SET ESCAPE OFF 
SET EXCLUSIVE OFF 
ON SHUTDOWN quit
SET DELETED on
PUBLIC enfoque, muser,mmodulo, mcodclie
muser = ""
mcodclie = 0
mmodulo = "DeltaGym"
enfoque=0
_screen.Caption="D-MonFingerPrint"
_screen.windowstate= 2
 
SET UDFPARMS TO VALUE
PUBLIC cMatriz 
PUBLIC bFP
bFP = .F.
SET UDFPARMS TO VALUE
DO "atask.prg"
SET UDFPARMS TO VALUE
DO "atask_ind.prg"
SET UDFPARMS TO VALUE
*DO "FindHwnd.prg"
Local Array arrVersion[1]
Agetfileversion(arrVersion, Application.ServerName)
_screen.Caption=arrVersion[10]+' Version '+ Alltrim(arrVersion[4])
DO FORM frmDFP
READ events
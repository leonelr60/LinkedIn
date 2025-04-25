Set-ExecutionPolicy RemoteSigned -Scope CurrentUser -Force
$port= new-Object System.IO.Ports.SerialPort COM6, 9600, none, 8,one
$port.RtsEnable=1
$port.open()
$port.Close()
exit
# Base on ProtoBuf3 Beta3, For Unity5
## Basic usage
* Download protoc.exe
[protoc-3.0.0-beta-3-win32.zip](https://github.com/google/protobuf/releases)
* Write .proto
```
syntax = "proto3";
package sapphire;
message login
{
  string username = 1;
  string passwd = 2;
}
```
* To generate C# files from your .proto files
```
protoc -I=$SRC_DIR --csharp_out=$DST_DIR $SRC_DIR/message.proto
```
* Copy cs file to unity project Assets folder

## Building
* Testing environment<br/>
  *Win 10*<br/>
  *Unity 5.3.3f1*<br/>
  *Visual Studio 2015 sp2*<br/>

## Testing
 1. Open this Project with unity
 2. Debug with VS
 3. Add breakpoint at TestProto.cs function Start()
 4. Run in unity
 5. Check variable data if correct

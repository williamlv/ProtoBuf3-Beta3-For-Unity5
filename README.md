# Base on ProtoBuf3 Beta3, For Unity5
* Download protoc.exe
[protoc-3.0.0-beta-3-win32.zip](https://github.com/google/protobuf/releases)
* Write message.proto
```
syntax = "proto3";
package sapphire;
message login
{
  string username = 1;
  string passwd = 2;
}
```
* Generate CSharp File
```
protoc -I=$SRC_DIR --csharp_out=$DST_DIR $SRC_DIR/message.proto
```

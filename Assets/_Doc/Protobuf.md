# ProtoBuf使用说明
## Server
### 1.安装
#### Linux (依赖C++编译器, go, git)
  * 下载 对应平台的Source https://github.com/google/protobuf
  * 解压 tar -zxvf file.tar.gz
  * 安装
  	+ ./configure --prefix=/usr/local/protobuf
  	+ 重新编译前记得clean
  	+ make
  	+ make check
  	+ make install
  * 路径配置 /etc/profile
  	+ export PATH=$PATH:/usr/local/protobuf/bin/
  	+ export PKG_CONFIG_PATH=/usr/local/protobuf/lib/pkgconfig/
  	+ source /etc/profile
  * 配置动态链接库路径
  	+ vim /etc/ld.so.conf
  	+ 插入：/usr/local/protobuf/lib
  	+ sudo ldconfig # refresh shared library cache.

#### Windows
  * 下载 [protoc-3.0.0-beta-3-win32.zip](https://github.com/google/protobuf/releases)
  * 下载 [nuget.exe](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe)
  * 将protoc.exe和nuget.exe配置到PATH环境变量
  * 下载[protobuf](https://github.com/google/protobuf)
  ```
  git clone git@github.com:google/protobuf.git
  ```
[//]: # (安装 VS2015 sp2)
[//]: # (装VS2015插件 NUnit, NuGet Packager, NuGet References, NuSpec Packager)
[//]: # (安装Microsoft .NET Framework 3.5 Service Pack 1完整软件包)
[//]: # (使用NuGet包管理器安装各项目中依赖的包，生成解决方案)
[//]: # (将生成后的dll等文件拷到Unity/Assets/Plugins目录下)


  * 待更新

### 2.消息文件 message.proto
```
syntax = "proto3";
package sapphire;
message login
{
  string username = 1;
  string passwd = 2;
}
```
#### 生成 go
```
protoc --go_out=. message.proto
```
#### 生成 CSharp
```
protoc -I=$SRC_DIR --csharp_out=$DST_DIR $SRC_DIR/message.proto
```
### 结构使用手册
官方链接 [结构使用手册](https://developers.google.com/protocol-buffers/docs/proto3)

#### syntax = "proto3";<br/>
第一行文件头必须指定使用版本，不能带注释
#### message SturctName <br/>
消息结构名，单词首字母均要大写，message结构可以嵌套，在另的文件定义的可以使用import ".proto"导入
#### 分配标识号
每个字段都有唯一的一个数字标识符，这些标识符是用来在消息的二进制格式中识别各个字段的，一旦开始使用就不能够再改变。[1,15]之内的标识号在编码的时候会占用一个字节。[16,2047]之内的标识号则占用2个字节。所以应该为那些频繁出现的消息元素保留 [1,15]之内的标识号。切记：要为将来有可能添加的、频繁出现的标识号预留一些标识号。不可以使用其中的[19000－19999]的标识号
#### 指定字段规则
- singular 可以有0或1个
- repeated 可以有0到多个，多个的顺序会保留。应该使用特殊选项[packed=true]来保证更高效的编码，数字型默认packed

#### 多个消息结构可以定义在一个proto文件中
#### 行后使用//注释
#### Reserved 指定被删除的tags或名字（保留）,防止以后重用这些字段tag或name引起错误
```
message Foo {
  reserved 2, 15, 9 to 11; //tag和名字要分开
  reserved "foo", "bar";
}
```
#### 基本类型
double、float、int32、int64、uint32、uint64、bool、string、bytes
int32 使用可变长编码方式。fixed32 总是4个字节。
编码负数时不够高效——如果你的字段可能含有负数，那么请使用sint32,sint64,sfixed32,sfixed64

#### 默认值
对string来说，默认值是空字符串。对bool来说，默认值是false。对数值类型来说，默认值是0。对枚举来说，默认值是枚举类型定义中的第一个值（0），其他message为null。repeated 为空列表,不能使用[default] 指定默认值了
#### enum
第一个值必须是0，枚举常量必须在32位整型值的范围内。因为enum值是使用可变编码方式的，对负数不够高效，因此不推荐在enum中使用负数
```
message SearchRequest {
    string query = 1;
    int32 page_number = 2;
    int32 result_per_page = 3;
  enum Corpus {
    UNIVERSAL = 0;
    WEB = 1;
  }
  Corpus corpus = 4;
}
```
可以为不同枚举成员定义相同常量值。，需要设置allow_alias option 为 true, 否则 protocol编译器会产生错误信息。
可以在 一个消息定义的内部或外部定义枚举——这些枚举可以在.proto文件中的任何消息定义里重用。当然也可以在一个消息中声明一个枚举类型，而在另一个不同 的消息中使用它——采用MessageType.EnumType的语法格式。

### 更新一个消息类型
如果一个已有的消息格式已无法满足新的需求——如，要在消息中添加一个额外的字段——但是同时旧版本写的代码仍然可用。不用担心！更新消息而不破坏已有代码是非常简单的。在更新时只要记住以下的规则即可。
- 不要更改任何已有的字段的数值标识
- 旧代码解析新消息包时新字段不可用，新代码解析旧消息包时新字段也不可用，只有默认值。
- 旧的tag可以删除，更好的做法是重命名字段名加个"OBSOLETE_"， 或是使用reserved限定。
- int32, uint32, int64, uint64,和bool是全部兼容的，类型不符会强制类型转换。
- sint32和sint64是互相兼容的，但是它们与其他整数类型不兼容。
- string和bytes是兼容的——只要bytes是有效的UTF-8编码。
- 嵌套消息与bytes是兼容的——只要bytes包含该消息的一个编码过的版本。
- fixed32与sfixed32是兼容的，fixed64与sfixed64是兼容的。
- enum is compatible with int32, uint32, int64, and uint64

### Any
代替了extensions;
```
import "google/protobuf/any.proto";
message ErrorStatus {
  string message = 1;
  repeated google.protobuf.Any details = 2;
}

// C++ 示例 Storing an arbitrary message type in Any.
NetworkErrorDetails details = ...;
ErrorStatus status;
status.add_details()->PackFrom(details);

// Reading an arbitrary message from Any.
ErrorStatus status = ...;
for (const Any& detail : status.details()) {
  if (detail.Is<NetworkErrorDetails>()) {
    NetworkErrorDetails network_error;
    detail.UnpackTo(&network_error);
    ... processing network_error ...
  }
}
```

### Oneof
如果你的消息中有很多可选字段， 并且同时至多一个字段会被设置， 你可以使用Oneof特性节省内存
可以使用case()或者WhichOneof() 方法检查哪个oneof字段被设置，
- 设置一个字段会清除别的字段
- 同一field被解析多次，只保留最后一次的值。
- 不能repeated
- 反射API可以识别
- C++注意crash，设置字段会删除别的字段
- 版本兼容问题，当增加或者删除oneof字段时一定要小心. 如果检查oneof的值返回None/NOT_SET, 它意味着oneof字段没有被赋值或者在一个不同的版本中赋值了。 你不会知道是哪种情况。
- Tag Reuse Issues
 + Move fields into or out of a oneof: You may lose some of your information (some fields will be cleared) after the message is serialized and parsed.
 + Delete a oneof field and add it back: This may clear your currently set oneof field after the message is serialized and parsed.
 + Split or merge oneof: This has similar issues to moving regular fields.

### Maps
map编写格式为 map<key_type, value_type> map_field = N;
- key_type 数字和字符串类型，不能是float,bytes.
- value_type 任意类型
- 不能repeated
- 不能依赖特定的顺序访问
- merge,重复赋值，相同的Key会保留最后一个
-
### Package
可以为proto声明一个package,防止不同的消息命名冲突

### RPC, gRPC

### Option

### Custom Options

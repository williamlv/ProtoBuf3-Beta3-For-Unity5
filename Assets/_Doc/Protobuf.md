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
第一行文件头必须指定使用版本，不带注释
#### message SturctName <br/>
消息结构名，单词首字母均要大写，message结构可以嵌套
#### 分配标识号
每个字段都有唯一的一个数字标识符，这些标识符是用来在消息的二进制格式中识别各个字段的，一旦开始使用就不能够再改变。[1,15]之内的标识号在编码的时候会占用一个字节。[16,2047]之内的标识号则占用2个字节。所以应该为那些频繁出现的消息元素保留 [1,15]之内的标识号。切记：要为将来有可能添加的、频繁出现的标识号预留一些标识号。不可以使用其中的[19000－19999]的标识号
#### 基本类型
double、float、int32、int64、uint32、uint64、bool、string、bytes
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
    IMAGES = 2;
    LOCAL = 3;
    NEWS = 4;
    PRODUCTS = 5;
    VIDEO = 6;
  }
  Corpus corpus = 4;
}
```
可以为枚举常量定义别名。 需要设置allow_alias option 为 true, 否则 protocol编译器会产生错误信息。

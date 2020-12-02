# Level 15 to Level 16

The code is vulnerable to blind SQL injection

Enter: `z" union select 1,password from users where username="natas16" #` as username

If we play around with `username="natas16"` we can see we get different messages

We could eventually try to guess the password
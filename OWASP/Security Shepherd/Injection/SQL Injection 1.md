# 1st step:

Try to make the page returning a database error by typing ```' or "```.
That will prove that the page is vulnerable.

# 2nd step:

Solution: ```" or 1 = 1#```

> "#" symbol comments the following text until the end of the line. 

Query must be something like:

```SELECT * FROM table WHERE column =" + USER_INPUT + ";```
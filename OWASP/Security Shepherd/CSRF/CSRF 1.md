# Requirements:

This level requires 2 users on the system

* Attacker
* Victim

# 1st step:

Login with the attacker user and post in the textbox the following:
[domain of website] + "/user/csrfchallengeone/plusplus?userid=" + [user id]

**example:**

```https://192.168.1.7/user/csrfchallengeone/plusplus?userid=532b1d7deaa78554d370967e15ca57d233ddd23c```

# 2nd step:

Login with the victim user, go to the CSRF 1 page, and just confirm that there's a message there from the attacker

# 3rd step:

Go back to the CSRF 1 page with the attacker account, and you should see the the result key there.
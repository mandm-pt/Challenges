# Requirements:

This level requires 2 users on the system

* Attacker
* Victim

# 1st step:

Create a custom html page that will be injected. The page should POST automatically to the address given and with the parameter userId=[attacker userId]

To get your user id, go to page CSRF 1

**example:**

[Check this example page](CSRF%202.html)

# 2nd step:

Publish/Host the page somewhere and inject the address of it in the textbox.

> In linux you can run the following command on the folder that contains your malicious html page: ```python3 -m http.server```

# 3rd step:

Once you submitted the page, login with the victim user, go to the CSRF 2 page, and just confirm that there's a message there from the attacker

# 4th step:

Go back to the CSRF 2 page with the attacker account, and you should see the the result key there.
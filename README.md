# HTTPayload
This is a Payload for windows that allows for a reverse HTTP Shell. I designed this program as a proof of concept as the HTTP traffic is very unlikely to be detected as being fraudulent from an Anti Virus / Firewall Perspective. I am working on a new version of this program that can bypass State-Full Firewalls.

## How it Works
1. The Attacker executes the HTTP Server as admin on a device
1. The attacker enters a command to run
1. The Payload is constantly listening for the attacker to send a message and once it has recieved the command, it stores it in a string
1. The Payload executes the command
1. The Payload sends an http request to the server with the header content being the response to the afore-mentioned command

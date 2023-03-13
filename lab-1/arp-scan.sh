#!/bin/bash
sudo ip -s -s neigh flush all
NETWORK=$(ip a show dev wlp4s0 | grep -Eo "([0-9]{1,3}\.){3}[0-9]{1,3}/[0-9]{1,2}")

echo "Scanning subnetwork $NETWORK"
# -sn : No port scan
# -PR : ARP scan
nmap -sn -PR $NETWORK

sleep 5

echo "all clients"
#cat /proc/net/arp 
cat /proc/net/arp | awk '{ if ($3 == "0x2") { print $1; ("host " $1 | getline result); print result "\t" $4}}'

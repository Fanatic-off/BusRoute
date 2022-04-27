# BusRoute

There are N buses, each bus travels along a pre-known cyclic route. 
The fare is known (paid at the entrance to the bus) and the travel time between stops. 
Buses enter the route (appear at the first stop) at a certain time.

It is necessary to write a program that will find two ways: the cheapest way and the fastest way. 
The program interface should allow you to download a file with bus routes, select the starting and ending points and the departure time from the starting point.

Input file format:

{N number of buses}
{K number of stops}
{departure time of 1 bus} {departure time of 2 buses} ... {departure time of N bus}
{fare for 1 bus} {fare for 2 buses} ... {fare on N bus}
{number of stops on the route of 1 bus} {stop number 1} {stop number 2} ... {last stop number} {travel time between 1 and 2 stops} {travel time between 2 and 3 stops}... {travel time between X and 1 stop}
... routes of other buses ...

Example:

2
4
10:00 12:00
10 20
2 1 3 5 7
3 1 2 4 10 5 20

Evidence:
1. The stops are numbered in a row from 1 to K.
2. The travel time between stops is set in minutes as an integer.
3. The fare is set in rubles as an integer.
4. Buses do not interfere with each other.
5. The bus does not waste time at the stop (costs 0 minutes).
6. The input file contains no errors.
7. All buses disappear at 00:00, i.e. all calculations take place before midnight.
# .NET test assignment

## Test Assignment
The purpose of this test assignment is to demonstrate the skills of building scalable and resilient services.
We will assess code structure, patterns applied, solution completeness and correctness.

## Description
Build a REST service to measure distance in miles between two airports. Airports are identified by 3-letter IATA code.
To get airport details HTTP call shall be used, sample for AMS airport:

``` shell
GET https://places-dev.cteleport.com/airports/AMS HTTP/1.1
```

It's allowed to use any 3-rd party components/frameworks. Solution has to be based on dotnet core 5.0+

# How to use 

```shell
curl -i -X GET "http://localhost:8081/api/v1/distance/calculate?from=lhr&to=ams"
```

# How To Build 

```shell 
docker-compose build 
docker-compose up
```

# Created Features 

## REST API 

### Features 

Endpoint `distance/calculate` 

The endpoint contaains two parameters that setup via query params: 
    - from
    - to 

The `from` parameter is a IATA code of first airport. 
The `to` parameter is a IATA code of second airport.

### Version 

The endpoint contains versioning feature. Versioning implemented via setup version in the query. 

### Distance Calculator 

The algorithm of calculation distance between airport has taken from https://en.wikipedia.org/wiki/Haversine_formula
Of course, it is possible to switch to another algo if implemented one. 

https://www.airportdistancecalculator.com to test that the number is correct. 

### Caching 

To boost calculation and reduce http requests, caching approach is used. 
There are two possible caching approaches implemented: 
    - InMemory 
    - Redis 

When use InMemory, then each service contains it is own cache. This is not scalable well. 
When use Redis, it is a distrebuted cache. Several services depends on the cache.

In the current implementatio, only Location coordinates is cached. 

# What Might Be Done Next 

## Features 

- Cache not only the location coordinates, but result of calculation of the distance;
- Calculate distance from West and East path from the first airport;
- Add LB and run several instances of the web api app  (nginx to make round-robbin)

## Refactoring 

There are 12_000 IATA codes approximately. 
Run a cron job to fetch data from the provided url. Persist data to internal storage. When up the web api app, fill the inmemory cache from the persistence storage. 
HashMap(Dictionary) would take approximatelly 300kb in memory. The only delay when service start to fill the cache. 



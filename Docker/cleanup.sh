#!/bin/bash

docker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)
docker system prune -a
docker network create -d bridge my-network
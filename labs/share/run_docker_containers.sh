#!/usr/bin/env bash

sudo docker run -td -p 6379:6379 redis
sudo docker run -td -p 4369:4369 rabbitmq


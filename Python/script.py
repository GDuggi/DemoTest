#from multiprocessing import Pool
import multiprocessing
import time

#import argparse

#import uuid

#import ssl

#import http.client

#import subprocess

import sys

import os

#import urllib.request

import json

def nodereboots(nodeList):

    #print("In method : {} ".format(nodeList))

    res=""

    for node in nodeList:

        print("In loop : {} ".format(node))

    return res

                                            

def main():

    #app_vars = init()

    #nodePath = '/v1/nodes'

    #print(https:// + app_vars["server"] + ":" + str(app_vars["port"]) + nodePath)
    f = open('/users/gouthamduggi/input.json').read()
    #with open('/users/gouthamduggi/input.json').read as f:
    nodeListstr = json.dumps(f)

    nodeList = json.loads(nodeListstr)
    #print("In Main : {} ".format(nodeList))
    #nodeListstr_res=json.loads(nodeListstr)

    start_time = time.perf_counter()

    #with Pool(5) as pool:

                   #reslut=pool.map(nodereboots,nodeList)

                   #print(reslut)
    p1 = multiprocessing.Process(target=nodereboots,args=[nodeList])
    p2 = multiprocessing.Process(target=nodereboots,args=[nodeList])
    p1.start()
    p2.start()
    p1.join()
    p2.join()
    #result = result_async.get()
    #print(result)
    finish_time = time.perf_counter()

    print("Program finished in {} seconds - using multiprocessing".format(finish_time-start_time))

    print("---")
    #f.close()

    #start_time = time.perf_counter()

    #nodereboots(nodeList)

    #finish_time = time.perf_counter()

    #print("Program finished in {} seconds - with out multiprocessing".format(finish_time-start_time))  

if __name__ == '__main__':

    main()

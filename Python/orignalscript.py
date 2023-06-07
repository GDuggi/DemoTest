#!/usr/bin/env python3

 

"""

Author  : Douglas Duncan

Date    : 2020-06-19

Purpose : Drain a group of nodes for maintenance

"""

 

import argparse

import uuid

import ssl

import http.client

#import subprocess

import sys

import os

import urllib.request

import json

import time

 

rundeckurl = https://rundeck.tools.mdgapp.net

rundecktoken = "bpOga70MppcSHePeESltPePjo2YCdFGE"

 

class job:

    def __init__(self,id,status):

        self.state = status

        self.id = id

 

 

# ---------------------------------------------------------------------------

def get_args():

    """ Get command line arguments """

 

    parser = argparse.ArgumentParser(

        description="Drain Nomad node",

        formatter_class=argparse.ArgumentDefaultsHelpFormatter

    )

 

    parser.add_argument('-e',

                        '--environment',

                        help='Environment to run in',

                        metavar='str',

                        type=str,

                        choices=['dev', 'qa', 'ptc', 'ctc'],

                        required=True,

                        default='')

 

    parser.add_argument('-d',

                        '--datacenter',

                        help='Nomad Data Center',

                        metavar='str',

                        type=str,

                        required=True,

                        default='')

 

    parser.add_argument('-t',

                        '--token',

                        help='Nomad token with `node:write` capabilities',

                       # metavar='str',

                        type=str,

                        required=True,

                        default='')

 

    parser.add_argument('-s',

                        '--seconds',

                        help='Number of seconds to wait before force stopping jobs',

                        metavar='int',

                        type=int,

                        default=3600)

 

    parser.add_argument('-i',

                        '--ignore',

                        help='Ignore system jobs',

                        metavar='str',

                        type=str,

                        choices=['true', 'false'],

                        default=False)

 

    parser.add_argument('-k',

                        '--ticket',

                        help='Ticket number needed for reboot',

                        metavar='str',

                        type=str,

                        default='')

    parser.add_argument('-c',

                        '--chef',

                        help='Run chef-client',

                        metavar='str',

                        type=str,

                        default='')

 

    parser.add_argument('-r',

                        '--reboot',

                        help='Execute a reboot of the node',

                        metavar='str',

                        type=str,

                        default='')

                       

    parser.add_argument('-l',

                        '--local',

                        help='Execute a local reboot of the node',

                        metavar='str',

                        type=str,

                        default='')

                       

    return parser.parse_args()

 

 

# ---------------------------------------------------------------------------

def init():

    """ Initializer """

 

    args = get_args()

 

    app_vars = {}           # Dictionary to hold veriables in use by the application

    app_vars['data'] = {}   # Dictionary to hold data t0 pass to the API

   

    # Validate the UUID token

    if not is_valid_uuid(args.token):

        print(f"{args.token} is not a valid UUID.")

        sys.exit(1)

    else:

        app_vars["token"] = args.token

   

    app_vars["environment"] = args.environment

 

    app_vars["datacenter"] = args.datacenter

 

    app_vars["ticket"] = args.ticket

   

    app_vars["local"] = args.local

 

    app_vars['data']['DrainSpec'] = {}  # Drain specification

    app_vars['data']['DrainSpec']['Deadline'] = args.seconds

    if args.ignore == 'true':

        ignore = True

    else:

        ignore = False

    if args.chef == 'true':

        chef = True

    else:

        chef = False

    if args.reboot == 'true':

        reboot = True

    else:

        reboot = False

    app_vars['chef'] = chef

    app_vars['reboot'] = reboot

    app_vars['data']['DrainSpec']['IgnoreSystemJobs'] = ignore

 

    # Set the context to not validate our self signed certs

    ctx = ssl.create_default_context()

    ctx.check_hostname = False

    ctx.verify_mode = ssl.CERT_NONE

    app_vars["ctx"] = ctx

 

    # Headers cent to the API

    app_vars["headers"] = {}

    app_vars["headers"]["X-Nomad-Token"] = app_vars["token"]

    app_vars["headers"]["Content-Type"] = "application/json; charset=UTF-8"

   

    # Build our server name based on the environment passed in

    app_vars["server"] = "nomad." + app_vars["environment"] + ".tools.mdgapp.net"

    app_vars["port"] = 443

 

    return app_vars

 

 

# ---------------------------------------------------------------------------

def is_valid_uuid(token):

    """ Validates token """

 

    try:

        uuid.UUID(token)

        return token == str(uuid.UUID(token))

    except Exception as exception:

        print(str(exception))

        return False

 

 

# ---------------------------------------------------------------------------

def api_connection(app_vars):

    """ Open a connection to the API """

 

    try:

        return http.client.HTTPSConnection(app_vars['server'],

                                          app_vars['port'],

                                          context=app_vars['ctx'])

    except ConnectionRefusedError:

        print(f"Could not connect to server {app_vars['server']} on port {app_vars['port']}.")

        sys.exit(1)

    except Exception as exception:

        print("An errr has occurred:")

        print(str(exception))

        sys.exit(1)

 

 

# ---------------------------------------------------------------------------

def api_request(conn, request_type, path, app_vars):

    """ Get the results from an API call """

 

    try:

        conn.request(request_type,

                     path,

                     json.dumps(app_vars['data']),

                     app_vars['headers'])

        response = conn.getresponse().read().decode("UTF-8")

        try:

            json.loads(response)

            return response

        except:

            print("The Job has failed:")

            print(response)

            sys.exit(1)

    except Exception as exception:

        print("The job has failed:")

        print(str(exception))

        sys.exit(1)

 

 

# ---------------------------------------------------------------------------

def GetRundeckStatus(runid):

    time.sleep(10)

    jobstat = "RUNNING"

    headers = {'Content-Type': 'application/json','X-Rundeck-Auth-Token': rundecktoken, 'cache-control': 'no-cache', 'Postman-Token': '97269834-899e-4b7f-8718-dbf75610ff26'}

    data = {

        "argString":"",

        "loglevel":"",

        "asUser":"",

        "filter":"",

        "runAtTime":"",

        "options":{}

        }

 

    data = json.dumps(data).encode('ascii')

    while jobstat == "RUNNING":

        url = rundeckurl + '/api/20/execution/' + str(runid) +'/state'

        req = urllib.request.Request(url,data,headers)

        with urllib.request.urlopen(req) as response:

            resp = json.loads(response.read())

            jobstat = (resp['executionState'])

        #    print (response.read())

        time.sleep(5)

    return job(runid,jobstat)

 

 

# ---------------------------------------------------------------------------

def StartRundeckJob(jobid,arguments):

    headers = {'Content-Type': 'application/json','X-Rundeck-Auth-Token': rundecktoken, 'cache-control': 'no-cache', 'Postman-Token': '97269834-899e-4b7f-8718-dbf75610ff26'}

 

    data = {

        "argString":"",

        "loglevel":"",

        "asUser":"",

        "filter":"",

        "runAtTime":"",

        "options":{}

        }

 

    for i in arguments.split(','):

        things = i.split('=')

        data['options'][things[0]] = things[1]

 

    data = json.dumps(data).encode('ascii')

    url = rundeckurl + '/api/20/job/' + jobid + '/run?format=json'

    req = urllib.request.Request(url,data,headers)

    with urllib.request.urlopen(req) as response:

        resp = json.loads(response.read())

        return (resp["id"])

 

 

# ---------------------------------------------------------------------------

 

def GenerateJobURL(jobid):

    return rundeckurl + "/project/InternalTools/execution/show/" + str(jobid) + "#output"

 

#----------------------------------------------------------------------------

 

def main():

    """ Main process """

 

    app_vars = init()

 

    conn = api_connection(app_vars)

    nodePath = '/v1/nodes'

 

    # Get a list of all clients

    print(https:// + app_vars["server"] + ":" + str(app_vars["port"]) + nodePath)

    nodeList = json.load(urllib.request.urlopen(https:// + app_vars["server"] + ":" + str(app_vars["port"]) + nodePath))

 

    for node in nodeList:

 

        # We only want to do the Nomad Data Center that was passed in

        if node['Datacenter'] == app_vars['datacenter']:

 

            print('Preparing to drain client {}.'.format(node["ID"]))

 

            # Rundeck `Drain Client` job

            stufftosend = "environment=" + app_vars["environment"] + ",node=" + node["ID"] + ",drain=start,seconds=" + str(app_vars['data']['DrainSpec']['Deadline']) + ",ignore=false"

 

            drainClient = GetRundeckStatus(StartRundeckJob("7158e7a7-956c-48ae-b8fe-424789c9c74b", stufftosend))

 

            if drainClient.state != "SUCCEEDED":

                print('There was an error running the `Drain Client` job ({}) on node {}'.format(drainClient.id, node["ID"]))

                print (GenerateJobURL(drainClient.id))

                exit(1)

            else:

                print (GenerateJobURL(drainClient.id))

            #Run Chef Client

            time.sleep(app_vars['data']['DrainSpec']['Deadline'])

            if app_vars['chef']:

                print ("Running chef...")

                runChefClient = GetRundeckStatus(StartRundeckJob("5cf2143d-0d8b-4598-a394-95cc71825b08","Server=" + node["Name"]))

 

                if runChefClient.state != "SUCCEEDED":

                    print('Chef Client Run failed for ' + node["Name"])

                    print (GenerateJobURL(runChefClient.id))

                    exit(1)

                else:

                    print (GenerateJobURL(runChefClient.id))

            else:

                print("Skipping Chef run")

 

            # Rundeck `Reboot VM` job

            if app_vars['reboot'] and app_vars['local'] == "false":

                print("Rebooting node...")

                rebootservername = node["Name"].split('.')[0]

                stufftosend = "ServerName=" + rebootservername + ",TicketID=" + app_vars["ticket"]

 

                rebootVM = GetRundeckStatus(StartRundeckJob("28993be7-b778-4679-b0f6-3c22b11b1876", stufftosend))

                if rebootVM.state != "SUCCEEDED":

                    print (GenerateJobURL(rebootVM.id))

                    print('There was an error running the `Reboot VM` job ({}) on node {}'.format(rebootVM.id, node["ID"]))

                    exit(1)

                else:

                    print (GenerateJobURL(rebootVM.id))

                    while os.system("ping -c 1 " + node["Name"]) == 0:

                        time.sleep(10)

                        print("Waiting for reboot...")

                    time.sleep(10)

                    print("Machine Rebooted")

                    while os.system("ping -c 1 " + node["Name"]) == 1:

                        time.sleep(30)

                        print("Waiting for machine to come back up...")

                    time.sleep(120)

            elif app_vars['reboot'] and app_vars['local'] == "true":

                localReboot = GetRundeckStatus(StartRundeckJob("47ac68f0-74d4-45e3-abc7-8ca9e4cad468","ServerName=" + node["Name"] + ",OS=linux"))

                if localReboot.state != "SUCCEEDED":

                    print('There was an error running the `Reboot VM` job ({}) on node {}'.format(rebootVM.id, node["ID"]))

                    print (GenerateJobURL(localReboot.id))

                    exit(1)

                else:

                    print("Machine Rebooted")

            else:

                print("Skipping reboot")

            # Rundeck `Set Client Eligibility` job

            stufftosend = "environment=" + app_vars["environment"] + ",node=" + node["ID"] + ",eligibility=eligible"

                

            setEligibility = GetRundeckStatus(StartRundeckJob("da69ed4c-c686-4416-ba31-8f68f0235e66", stufftosend))

 

            if setEligibility.state != "SUCCEEDED":

                print('There was an error running the `Set Client Eligibility` job ({}) on node {}'.format(setEligibility.id, node["ID"]))

                exit(1)

 

 

# ---------------------------------------------------------------------------

if __name__ == '__main__':

    main()


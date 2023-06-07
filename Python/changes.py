from multiprocessing import Pool
def enablescheduler(jobid):
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
    url = rundeckurl + '/api/20/job/' + jobid + '/schedule/enable'
    req = urllib.request.Request(url,data,headers)
    with urllib.request.urlopen(req) as response:
        resp = json.loads(response.read())
        return (resp["id"])


def disablescheduler(jobid):
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
    url = rundeckurl + '/api/20/job/' + jobid + '/schedule/disable'
    req = urllib.request.Request(url,data,headers)
    with urllib.request.urlopen(req) as response:
        resp = json.loads(response.read())
        return (resp["id"])
		

		
def nodereboots(nodelist):
	for node in nodelist:
		if node['Datacenter'] == app_vars['datacenter']:
			print(node['Name'])
			stufftosend = "environment=" + app_vars["environment"] + ",node=" + node["ID"] + ",drain=start,seconds=" + str(app_vars['data']['DrainSpec']['Deadline']) + ",ignore=false"
			disablescheduler("7158e7a7-956c-48ae-b8fe-424789c9c74b", stufftosend)
            drainClient = GetRundeckStatus(StartRundeckJob("7158e7a7-956c-48ae-b8fe-424789c9c74b", stufftosend))
			#stuff need to implemnt the condtion for drainstatus 
			rebootVM = GetRundeckStatus(StartRundeckJob("28993be7-b778-4679-b0f6-3c22b11b1876", stufftosend))
			#stuff need to implemnt the condtion for reboot status
			enablescheduler("7158e7a7-956c-48ae-b8fe-424789c9c74b", stufftosend)

def main():
	with Pool(5) as pool:
	pool.map(nodereboots,nodelist)

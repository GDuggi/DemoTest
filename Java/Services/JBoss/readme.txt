Notes about Confirmations project

Apache Ant(TM) version 1.9.0 compiled on March 5 2013
in ant LIB add  ivy-2.4.0.jar
in ant LIB add  ant-contrib-1.0b3.jar
in ant LIB add  saxon9he.jar

* Control where ant puts its build products
Create file c:\users\<your-user-name>\build.properties containing:
   build.dir=c:/aff/build/${ant.project.name}
   ivy.repo.local=c:/aff/repo

* To make first build
$cd Java\Services\JBoss
$ant 
$ant ide
* or 
$ant build ide

* To build quickly - skips unzip and zip steps
$cd Java\Services\JBoss
$ant quick

* Some modules like JBoss take a long time because of the unzip/zip steps
$cd Java\Services\JBoss\jboss
$ant quick

* To get a CLEAN build
$cd Java\Services\JBoss\jboss
$ant clean
$ant build
* or 
$ant clean build
* or 
$ant all

* To build a module
$cd Java\Services\JBoss\common-service
$ant 

* Now get the compiled common-services into the jboss distribution (this is not a full build)
$cd Java\Services\JBoss\jboss

$ant ivy-retrieve deploy module add
or
$ant
or
$ant quick

* Run the distribution we created (see Control where ant puts its build products above)
$cd \aff\build\cnf-JBoss\dist\bin
$r

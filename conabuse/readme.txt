lookcool -----
   "conabuse"

RNG based fake logger

   arguments:
     -iFILE : multiline input file containing strings
	 -tSTRING : add string to logger (can be applied multiple times)
	 -fDOUBLE : logging frequency : 0.0 - 1.0
	 -wINT : enable wait method (see below for details)
	 -oINT : start position in console

     -giDOUBLE : glitch intensity : 0.0 - 1.0
	 -gcINT    : glitch count
	   depending on how this is used,
	   it will draw random characters
	   in random positions

	 -sDOUBLE : amount of seconds to wait if -w1
	 -cINT : amount of iterations to wait if -w2
	 -bINT :      amount of loops to wait if -w3

     -mc : multicolor lines
	 -mcb : multicolor background

     -r : enable random string formatting
	        (use if %s or {#}s are encountered in inputs)

   wait methods (-w):
     1: sleep : waits using sleep function and timespan ticks
	 2: cpu spins : waits a specific amount of iterations
	 3: for buffer : waits using a blank for loop

	 sleeping is enabled by default

     append argument with "x" to disable a wait method

     warning, cpu spin waiting and for buffers
	 may be resource intensive based on specific use

   any unintentional errors by 
   the program may be used as output
   this can be turned off with -ned

   example use:
     conabuse -ikrnlstr -ioutput.log -f0.7 -o100
	   -gi0.4 -gc40 -s0.06 -w1x -w2 -c 200000

  programmed
    by Wesley

  http://donnaken15.tk
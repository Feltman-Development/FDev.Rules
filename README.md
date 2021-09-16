# FDev.Rules
ABOUT THE PROJECT - MAIN GUIDELINES
-----------------------------------
- Choose any one of the 2 problems.
- Can be written in your preferred coding language. 
- The implementation should adopt OOPS, clean code practices, SOLID principles, and best practices.
- Should be accompanied by unit tests (unit test is mandatory) & preferably written using TDD approach.
- Should commit code to a public git repository (github) under a public handle
- The codebase should be checked in properly in GitHub. Please DON’T zip and upload. Zip attachment will be straight away rejected.
- Avoid high cyclometric complexity.
- Use generic package names; don’t reference Maersk or any other Maersk brand
- Commits should be incremental so that one can look at the commit log and make sense of how the code has progressed along with the test cases 
- Recommended at least up to 20 commits to show how the code progresses
- Larger number of commits isn’t a problem
- The promotion rules are mutually exclusive, that implies only one promotion (individual SKU or combined) is applicable to a particular SKU. The rest depends on the imagination of the programmer for which scenarios they want to consider, for example (case 1 => 2A = 30 and A=A40%) or (case 2 => either 2A = 30 or A=A40%)
- Dont spend more than 1.5 hours - 2 hours. The important thing is to understand how the code shapes up and not to cover the entire range of spelling
    - On what? There is no way this is a less than a day project, even when you haven't gone overboard as I have:

ABOUT THE PROJECT - SCOPE (GONE CRAZY)
--------------------------------------
It have to be said: On the background of 

- missing a real code-project that wasn't one of my hobby project, and 
- encountering a pattern that I have not implemented before: RuleEngine (and not realising how immensely complex such an implementation can be),
- deciding to write my own RuleEngine from scratch, and
- deciding that it shall be dynamic, absolutely configurable, extensible and do just about everything - like solve both problems presented

I have gone totally overboard in the scope of the project and its implementations, but have had great fun and learned much - including knowledge that I
will be able too use in quite much in the future I presume, so it is all good for me and I hope that you will accept a solution that is a little more complex
than is asked for. I have done my best to hide the complexety in the code and made it as maintainable and understandable as possible, which has been no small task either. Anyway, hope you will enjoy reviewing the code as much as I have enjoyed writing it :-) 

THE PROBLEMS/TASKS
-------------------
PROBLEM 1  
- A company needs to implement a Promotion Engine in their check-out process, for calculation of the promotions given an shopping cart of products and a range of available promotions.
- I have chosen that the system shall be capable of calculating the optimal use of the promotions. Random applying of promotions does not make any sense. 
- System also calculates the total order value with discounts. 
- 3 scenarios with results are provided, to help test that the engine is calculating correctly.

PROBLEM 2
- A large company is processing a set of business rules manually regarding their order handling, and would like a system (a RuleEngine seems to be the pattern to apply) automating the process, so that a given condition will trigger a given action. With a Point Of Sale system being way out of the already proportionally warped scope (see below), the actions in the system will amount to verbal expressions.

PROBLEM 1 & 2
- In both cases, the importance of the systems maintainability and even more so, extensibility, is emphasized. And in relation to the real world this makes perfect sense, as these are systems that should be expected to require change and extension often. With that in mind, I thought that a system (that would be the RuleEngine) that truly lives up to the requirements set, should be so configurable, open for extensions and generics, that by using the same Engine and applying different sets of rules, should be able to solve both problems with at least 80% of the code being shared. So that is what I will try and do: Create one system that is able to solve both problems!

ABOUT THE PROJECT - THE WAY OF WORKING
--------------------------------------
In addition to the problems descriptions, the expected coding process (way of working) is pretty well-defined: To make it possible to follow your coding process, which after all could be more important than the pure result of your work, there is somewhat strange requirement of having at least 20 commits to your repository, which is a pretty hefty number for problems this size and a estimate of max 3 workdays. But I found a way to break the tasks down to small enough bits to hit the required number of commits - and a way to extend the task somewhat too, by taking on both problems - and it was actually a meaningfull learning experience on both accounts :-)

ABOUT THE PROJECT - 3RD PARTY LIBRTARIES
----------------------------------------
In my daily work I of course use a lot of 3rd party libraries and templates, none more than my own where I have templates and examples of boilerplate code and scaffolding for some patterns, which with some copy, paste, customizing and extending saves me a lot of time that I mainly use for two things:
- 1: Having more fun inventing stuff, like in this case: write my own   rule-engine from scratch.  
- 2: Taking more time writing clean, maintainable code with the level of documentation I would expect from any commercial code library.
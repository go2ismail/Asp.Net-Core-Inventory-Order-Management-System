var treeGridDataSource = [
           {
               "Name": "Windows",
               "DateModified": new Date("06/26/2017"),
               "Type": "File Folder",
               "DateCreated": new Date("06/16/2017"),
               "Children": [
                   {
                       "Name": "Users",
                       "DateModified": new Date("06/26/2017"),
                       "Type": "File Folder",
                       "DateCreated": new Date("06/16/2017"),
                       "Children": [
                           {
                               "Name": "Public",
                               "DateModified": new Date("06/26/2017"),
                               "Type": "File Folder",
                               "DateCreated": new Date("06/16/2017"),
                               "Children": [
                                   {
                                       "Name": "Documents ",
                                       "DateModified": new Date("06/26/2017"),
                                       "Type": "File Folder",
                                       "DateCreated": new Date("06/16/2017"),
                                       "Children": [
                                           { "Name": "Document 1", "DateModified": new Date("06/26/2017"), "Type": "HTML document", "DateCreated": new Date("06/16/2017"), },
                                           { "Name": "Document 2", "DateModified": new Date("06/26/2017"), "Type": "HTML document", "DateCreated":new Date( "06/16/2017"), },
                                           { "Name": "Document 3", "DateModified": new Date("06/26/2017"), "Type": "HTML document", "DateCreated": new Date("06/16/2017"), }
                                       ]
                                   }
                               ]
                           }
                       ]
                   }
               ]
           }
];

var sampleData = [
     {
         taskID: 1,
         taskName: "Planning",
         startDate: new Date("02/03/2017"),
         endDate: new Date("02/07/2017"),
         progress: 100,
         duration: 5,
         priority: "Normal",
         approved: false,
         subtasks: [
             { taskID: 2, taskName: "Plan timeline", startDate: new Date("02/03/2017"), endDate: new Date("02/07/2017"), duration: 5, progress: 100, priority: "Normal", approved: false },
             { taskID: 3, taskName: "Plan budget", startDate: new Date("02/03/2017"), endDate: new Date("02/07/2017"), duration: 5, progress: 100, approved: true },
             { taskID: 4, taskName: "Allocate resources", startDate: new Date("02/03/2017"), endDate: new Date("02/07/2017"), duration: 5, progress: 100, priority: "Critical", approved: false },
             { taskID: 5, taskName: "Planning complete", startDate: new Date("02/07/2017"), endDate: new Date("02/07/2017"), duration: 0, progress: 0, priority: "Low", approved: true }
         ]
     },
     {
         taskID: 6,
         taskName: "Design",
         startDate: new Date("02/10/2017"),
         endDate: new Date("02/14/2017"),
         duration: 3,
         progress: 86,
         priority: "High",
         approved: false,
         subtasks: [
             { taskID: 7, taskName: "Software Specification", startDate: new Date("02/10/2017"), endDate: new Date("02/12/2017"), duration: 3, progress: 60, priority: "Normal", approved: false },
             { taskID: 8, taskName: "Develop prototype", startDate: new Date("02/10/2017"), endDate: new Date("02/12/2017"), duration: 3, progress: 100, priority: "Critical", approved: false },
             { taskID: 9, taskName: "Get approval from customer", startDate: new Date("02/13/2017"), endDate: new Date("02/14/2017"), duration: 2, progress: 100, approved: true },
             { taskID: 10, taskName: "Design Documentation", startDate: new Date("02/13/2017"), endDate: new Date("02/14/2017"), duration: 2, progress: 100, approved: true },
             { taskID: 11, taskName: "Design complete", startDate: new Date("02/14/2017"), endDate: new Date("02/14/2017"), duration: 0, progress: 0, priority: "Normal", approved: true }
         ]
     },
	 {
	     taskID: 12,
	     taskName: "Implementation Phase",
	     startDate: new Date("02/17/2017"),
	     endDate: new Date("02/27/2017"),
	     priority: "Normal",
	     approved: false,
	     duration: 11,
	     subtasks: [
             {
                 taskID: 13,
                 taskName: "Phase 1",
                 startDate: new Date("02/17/2017"),
                     endDate: new Date("02/27/2017"),
                 priority: "High",
                 approved: false,
                 duration: 11,
                 subtasks: [{
                     taskID: 14,
                     taskName: "Implementation Module 1",
                     startDate: new Date("02/17/2017"),
                         endDate: new Date("02/27/2017"),
                     priority: "Normal",
                     duration: 11,
                     approved: false,
                     subtasks: [
                         { taskID: 15, taskName: "Development Task 1", startDate: new Date("02/17/2017"), endDate: new Date("02/19/2017"), duration: 3, progress: "50", priority: "High", approved: false },
                         { taskID: 16, taskName: "Development Task 2", startDate: new Date("02/17/2017"), endDate: new Date("02/19/2017"), duration: 3, progress: "50", priority: "Low", approved: true },
                         { taskID: 17, taskName: "Testing", startDate: new Date("02/20/2017"), endDate: new Date("02/21/2017"), duration: 2, progress: "0", priority: "Normal", approved: true },
                         { taskID: 18, taskName: "Bug fix", startDate: new Date("02/24/2017"), endDate: new Date("02/25/2017"), duration: 2, progress: "0", priority: "Critical", approved: false },
                         { taskID: 19, taskName: "Customer review meeting", startDate: new Date("02/26/2017"), endDate: new Date("02/27/2017"), duration: 2, progress: "0", priority: "High", approved: false },
                         { taskID: 20, taskName: "Phase 1 complete", startDate: new Date("02/27/2017"), endDate: new Date("02/27/2017"), duration: 0, priority: "Low", approved: true }

                     ]
                 }]
             },

             {
                 taskID: 21,
                 taskName: "Phase 2",
                 startDate: new Date("02/17/2017"),
                 endDate: new Date("02/28/2017"),
                 priority: "High",
                 approved: false,
                 duration: 12,
                 subtasks: [{
                     taskID: 22,
                     taskName: "Implementation Module 2",
                     startDate: new Date("02/17/2017"),
                     endDate: new Date("02/28/2017"),
                     priority: "Critical",
                     approved: false,
                     duration: 12,
                     subtasks: [
                         { taskID: 23, taskName: "Development Task 1", startDate: new Date("02/17/2017"), endDate: new Date("02/20/2017"), duration: 4, progress: "50", priority: "Normal", approved: true },
                         { taskID: 24, taskName: "Development Task 2", startDate: new Date("02/17/2017"), endDate: new Date("02/20/2017"), duration: 4, progress: "50", priority: "Critical", approved: true },
                         { taskID: 25, taskName: "Testing", startDate: new Date("02/21/2017"), endDate: new Date("02/24/2017"), duration: 2, progress: "0", priority: "High", approved: false },
                         { taskID: 26, taskName: "Bug fix", startDate: new Date("02/25/2017"), endDate: new Date("02/26/2017"), duration: 2, progress: "0", priority: "Low", approved: false },
                         { taskID: 27, taskName: "Customer review meeting", startDate: new Date("02/27/2017"), endDate: new Date("02/28/2017"), duration: 2, progress: "0", priority: "Critical", approved: true },
                         { taskID: 28, taskName: "Phase 2 complete", startDate: new Date("02/28/2017"), endDate: new Date("02/28/2017"), duration: 0, priority: "Normal", approved: false }

                     ]
                 }]
             },

             {
                 taskID: 29,
                 taskName: "Phase 3",
                 startDate: new Date("02/17/2017"),
                 endDate: new Date("02/27/2017"),
                 priority: "Normal",
                 approved: false,
                 duration: 11,
                 subtasks: [{
                     taskID: 30,
                     taskName: "Implementation Module 3",
                     startDate: new Date("02/17/2017"),
                         endDate: new Date("02/27/2017"),
                     priority: "High",
                     approved: false,
                     duration: 11,
                     subtasks: [
                         { taskID: 31, taskName: "Development Task 1", startDate: new Date("02/17/2017"), endDate: new Date("02/19/2017"), duration: 3, progress: "50", priority: "Low", approved: true },
                         { taskID: 32, taskName: "Development Task 2", startDate: new Date("02/17/2017"), endDate: new Date("02/19/2017"), duration: 3, progress: "50", priority: "Normal", approved: false },
                         { taskID: 33, taskName: "Testing", startDate: new Date("02/20/2017"), endDate: new Date("02/21/2017"), duration: 2, progress: "0", priority: "Critical", approved: true },
                         { taskID: 34, taskName: "Bug fix", startDate: new Date("02/24/2017"), endDate: new Date("02/25/2017"), duration: 2, progress: "0", priority: "High", approved: false },
                         { taskID: 35, taskName: "Customer review meeting", startDate: new Date("02/26/2017"), endDate: new Date("02/27/2017"), duration: 2, progress: "0", priority: "Normal", approved: true },
                         { taskID: 36, taskName: "Phase 3 complete", startDate: new Date("02/27/2017"), endDate: new Date("02/27/2017"), duration: 0, priority: "Critical", approved: false },

                     ]
                 }]
             }
	     ]
	 }
];

var templateData = [{
    "Name": "Robert King",
    "FullName": "Robert King",
    "Designation": "Chief Executive Officer",
    "EmployeeID": "EMP001",
    "Address": "507 - 20th Ave. E.Apt. 2A, Seattle",
    "Contact": "(206) 555-9857",
    "Country": "USA",
    "DOB": new Date("2/15/1963"),
    "DOJ": new Date("5/1/1983"),

    "Children": [{
        "Name": "David william",
        "FullName": "David william",
        "Designation": "Vice President",
        "EmployeeID": "EMP004",
        "Address": "722 Moss Bay Blvd., Kirkland",
        "Country": "USA",
        "Contact": "(206) 555-3412",
        "DOB": new Date("5/20/1971"),
        "DOJ": new Date("5/1/1991"),


        "Children": [{
            "Name": "Nancy Davolio",
            "FullName": "Nancy Davolio",
            "Designation": "Marketing Executive",
            "EmployeeID": "EMP035",
            "Address": "4110 Old Redmond Rd., Redmond",
            "Country": "USA",
            "Contact": "(206) 555-8122",
            "DOB": new Date("3/19/1966"),
            "DOJ": new Date("5/1/1986"),
            "Children": [
                {
                    "Name": "Andrew Fuller",
                    "FullName": "Andrew Fuller",
                    "Designation": "Sales Representative",
                    "EmployeeID": "EMP045",
                    "Address": "14 Garrett Hill, London",
                    "Country": "UK",
                    "Contact": "(71) 555-4848",
                    "DOB": new Date("9/20/1980"),
                    "DOJ": new Date("5/1/2000"),
                },
            {
                "Name": "Anne Dodsworth",
                "FullName": "Anne Dodsworth",
                "Designation": "Sales Representative",
                "EmployeeID": "EMP091",
                "Address": "4726 - 11th Ave. N.E., Seattle",
                "Country": "USA",
                "Contact": "(206) 555-1189",
                "DOB": new Date("10/19/1989"),
                "DOJ": new Date("5/1/2009"),
            },
            {
                "Name": "Michael Suyama",
                "FullName": "Michael Suyama",
                "Designation": "Sales Representative",
                "EmployeeID": "EMP110",
                "Address": "Coventry House Miner Rd., London",
                "Country": "UK",
                "Contact": "(71) 555-3636",
                "DOB": new Date("11/02/1987"),
                "DOJ": new Date("5/1/2007"),
            },
            {
                "Name": "Janet Leverling",
                "FullName": "Janet Leverling",
                "Designation": "Sales Coordinator",
                "EmployeeID": "EMP131",
                "Address": "Edgeham Hollow Winchester Way, London",
                "Country": "UK",
                "Contact": "(71) 555-3636",
                "DOB": new Date("11/06/1990"),
                "DOJ": new Date("5/1/2010"),
            },
            ]

        },
        {
            "Name": "Romey Wilson",
            "FullName": "Romey Wilson",
            "Designation": "Sales Executive",
            "EmployeeID": "EMP039",
            "Address": "7 Houndstooth Rd., London",
            "Country": "UK",
            "Contact": "(71) 555-3690",
            "DOB": new Date("02/02/1980"),
            "DOJ": new Date("5/1/2000"),
            "Children": [
            {
                "Name": "Margaret Peacock",
                "FullName": "Margaret Peacock",
                "Designation": "Sales Representative",
                "EmployeeID": "EMP213",
                "Address": "4726 - 11th Ave. N.E., California",
                "Country": "USA",
                "Contact": "(206) 555-1989",
                "DOB": new Date("01/21/1986"),
                "DOJ": new Date("5/1/2006"),

            },
            {
                "Name": "Laura Callahan",
                "FullName": "Laura Callahan",
                "Designation": "Sales Coordinator",
                "EmployeeID": "EMP201",
                "Address": "Coventry House Miner Rd., London",
                "Country": "UK",
                "Contact": "(71) 555-2222",
                "DOB": new Date("12/01/1990"),
                "DOJ": new Date("5/1/2010"),
            },
            {
                "Name": "Steven Buchanan",
                "FullName": "Steven Buchanan",
                "Designation": "Sales Representative",
                "EmployeeID": "EMP197",
                "Address": "200 Lincoln Ave, Salinas, CA 93901",
                "Country": "USA",
                "Contact": "(831) 758-7408",
                "DOB": new Date("03/23/1987"),
                "DOJ": new Date("5/1/2007"),
            },
            {
                "Name": "Tedd Lawson",
                "FullName": "Tedd Lawson",
                "Designation": "Sales Representative",
                "EmployeeID": "EMP167",
                "Address": "200 Lincoln Ave, Salinas, CA 93901",
                "Country": "USA",
                "Contact": "(831) 758-7368 ",
                "DOB": new Date("08/09/1989"),
                "DOJ": new Date("5/1/2009"),
            },
            ]
        }]
    }]
}];

var summaryRowData = [{
    "FreightID": "CX2389NK",
    "FreightName": "Maersk Edibles Co.",
    "TotalUnits": 598,
    "TotalCosts": 27838,
    "UnitWeight": 241,
    "children": [{

        "FreightID": "QW4567OP",
        "FreightName": "Chang",
        "TotalUnits": 123,
        "TotalCosts": 3400,
        "UnitWeight": 50,
    }, {
        "FreightID": "QW3458BH",
        "FreightName": "Aniseed Syrup",
        "TotalUnits": 89,
        "TotalCosts": 5900,
        "UnitWeight": 87,
    }, {
        "FreightID": "QW8967OH",
        "FreightName": "Chef Anton's Cajun Seasoning",
        "TotalUnits": 46,
        "TotalCosts": 9578,
        "UnitWeight": 54,
    }, {

        "FreightID": "QW6549NJ",
        "FreightName": "Chef Anton's Gumbo Mix",
        "TotalUnits": 340,
        "TotalCosts": 8960,
        "UnitWeight": 50,
    }]
},
                     {
                         "FreightID": "DW8954IO",
                         "FreightName": "Aeon fitness inc.",
                         "TotalUnits": 1720,
                         "TotalCosts": 24367,
                         "UnitWeight": 296,
                         "children": [
                             {
                                 "FreightID": "UF5647YH",
                                 "FreightName": "Reebox CrossFit Back Bay",
                                 "TotalUnits": 600,
                                 "TotalCosts": 8700,
                                 "UnitWeight": 73,
                             },
                           {
                               "FreightID": "UF1290LK",
                               "FreightName": "The Green Microgym",
                               "TotalUnits": 569,
                               "TotalCosts": 8765,
                               "UnitWeight": 90,
                           },
                         {
                             "FreightID": "UF8956KU",
                             "FreightName": "DeFranco's",
                             "TotalUnits": 456,
                             "TotalCosts": 4589,
                             "UnitWeight": 68,
                         },
                         {
                             "FreightID": "UF7464JK",
                             "FreightName": "Westside Barbell",
                             "TotalUnits": 95,
                             "TotalCosts": 2313,
                             "UnitWeight": 65,
                         }],
                     },
                     {
                         "FreightID": "EJ9456KN",
                         "FreightName": "Sun technologies inc",
                         "TotalUnits": 331,
                         "TotalCosts": 22933,
                         "UnitWeight": 192,
                         "children": [
                             {
                                 "FreightID": "GH2367OP",
                                 "FreightName": "Haier Group",
                                 "TotalUnits": 78,
                                 "TotalCosts": 6789,
                                 "UnitWeight": 23,
                             },
                           {
                               "FreightID": "GH4309TH",
                               "FreightName": "Panda Electronics",
                               "TotalUnits": 90,
                               "TotalCosts": 8999,
                               "UnitWeight": 48,
                           },
                         {
                             "FreightID": "GH3494SD",
                             "FreightName": "Jiangsu Etern",
                             "TotalUnits": 36,
                             "TotalCosts": 4356,
                             "UnitWeight": 56,
                         },
                         {
                             "FreightID": "GH3213FR",
                             "FreightName": "Zhejiang Fuchunjiang",
                             "TotalUnits": 127,
                             "TotalCosts": 2789,
                             "UnitWeight": 65,
                         }],

                     }];

var headerData = [
    {
        taskID: 1,
        taskName: "Planning",
        startDate: new Date("02/03/2017"),
        endDate: new Date("02/07/2017"),
        subtasks: [
            { taskID: 2, taskName: "Plan timeline", startDate: new Date("02/03/2017"), endDate: new Date("02/07/2017"), duration: 5, progress: "100", resourceId: "2" },
            { taskID: 3, taskName: "Plan budget", startDate: new Date("02/03/2017"), endDate: new Date("02/07/2017"), duration: 5, progress: "100", resourceId:"1" },
            { taskID: 4, taskName: "Allocate resources", startDate: new Date("02/03/2017"), endDate: new Date("02/07/2017"), duration: 5, progress: "100", resourceId: "1" },
            { taskID: 5, taskName: "Planning complete", startDate: new Date("02/07/2017"), endDate: new Date("02/07/2017"), duration: 0, predecessor: "3FS,4FS,5FS" }
        ]
    },
     {
         taskID: 6,
         taskName: "Design",
         startDate: new Date("02/10/2017"),
         endDate: new Date("02/14/2017"),
         subtasks: [
             { taskID: 7, taskName: "Software Specification", startDate: new Date("02/10/2017"), endDate: new Date("02/12/2017"), duration: 3, progress: "60", predecessor: "6FS", resourceId: "2" },
             { taskID: 8, taskName: "Develop prototype", startDate: new Date("02/10/2017"), endDate: new Date("02/12/2017"), duration: 3, progress: "100", predecessor: "6FS", resourceId: "3" },
             { taskID: 9, taskName: "Get approval from customer", startDate: new Date("02/13/2017"), endDate: new Date("02/14/2017"), duration: 2, progress: "100", predecessor: "9FS", resourceId: "1" },
             { taskID: 10, taskName: "Design complete", startDate: new Date("02/14/2017"), endDate: new Date("02/14/2017"), duration: 0, predecessor: "10FS" }
         ]
     },
     {
         taskID: 11,
         taskName: "Implementation Phase",
         startDate: new Date("02/17/2017"),
         endDate: new Date("02/27/2017"),
         subtasks: [
             {
                 taskID: 12,
                 taskName: "Phase",
                 startDate: new Date("02/17/2017"),
                 endDate: new Date("02/27/2017"),
                 subtasks: [{
                     taskID: 13,
                     taskName: "Implementation Module",
                     startDate: new Date("02/17/2017"),
                     endDate: new Date("02/27/2017"),
                     subtasks: [
                         { taskID: 14, taskName: "Development Task 1", startDate: new Date("02/17/2017"), endDate: new Date("02/19/2017"), duration: 3, progress: "50", predecessor: "11FS", resourceId: "3" },
                         { taskID: 15, taskName: "Development Task 2", startDate: new Date("02/17/2017"), endDate: new Date("02/19/2017"), duration: 3, progress: "50", predecessor: "11FS", resourceId: "3" },
                         { taskID: 16, taskName: "Testing", startDate: new Date("02/20/2017"), endDate: new Date("02/21/2017"), duration: 2, progress: "0", predecessor: "15FS,16FS", resourceId: "4" },
                         { taskID: 17, taskName: "Bug fix", startDate: new Date("02/24/2017"), endDate: new Date("02/25/2017"), duration: 2, progress: "0", predecessor: "17FS", resourceId: "4" },
                         { taskID: 18, taskName: "Customer review meeting", startDate: new Date("02/26/2017"), endDate: new Date("02/27/2017"), duration: 2, progress: "0", predecessor: "18FS", resourceId: "1" },
                         { taskID: 19, taskName: "Phase complete", startDate: new Date("02/27/2017"), endDate: new Date("02/27/2017"), duration: 0, predecessor: "19FS" }
                     ]
                 }]
             }
         ]
     }
];

var projectResources = [
    { resourceId: 1, resourceName: "Project Manager" },
    { resourceId: 2, resourceName: "Software Analyst" },
    { resourceId: 3, resourceName: "Developer" },
    { resourceId: 4, resourceName: "Testing Engineer" }
];

var pagingdata = [
     {
         taskID: 1,
         taskName: "Planning",
         startDate: new Date("02/03/2017"),
         endDate: new Date("02/07/2017"),
         progress: 100,
         duration: 5,
         priority: "Normal",
         approved: false,
         subtasks: [
             { taskID: 2, taskName: "Plan timeline", startDate: new Date("02/03/2017"), endDate: new Date("02/07/2017"), duration: 5, progress: 100, priority: "Normal", approved: false },
             { taskID: 3, taskName: "Plan budget", startDate: new Date("02/03/2017"), endDate: new Date("02/07/2017"), duration: 5, progress: 100, approved: true },
             { taskID: 4, taskName: "Allocate resources", startDate: new Date("02/03/2017"), endDate: new Date("02/07/2017"), duration: 5, progress: 100, priority: "Critical", approved: false },
             { taskID: 5, taskName: "Planning complete", startDate: new Date("02/07/2017"), endDate: new Date("02/07/2017"), duration: 0, progress: 0, priority: "Low", approved: true }
         ]
     },
     {
         taskID: 6,
         taskName: "Design",
         startDate: new Date("02/10/2017"),
         endDate: new Date("02/14/2017"),
         duration: 3,
         progress: 86,
         priority: "High",
         approved: false,
         subtasks: [
             { taskID: 7, taskName: "Software Specification", startDate: new Date("02/10/2017"), endDate: new Date("02/12/2017"), duration: 3, progress: 60, priority: "Normal", approved: false },
             { taskID: 8, taskName: "Develop prototype", startDate: new Date("02/10/2017"), endDate: new Date("02/12/2017"), duration: 3, progress: 100, priority: "Critical", approved: false },
             { taskID: 9, taskName: "Get approval from customer", startDate: new Date("02/13/2017"), endDate: new Date("02/14/2017"), duration: 2, progress: 100, approved: true },
             { taskID: 10, taskName: "Design Documentation", startDate: new Date("02/13/2017"), endDate: new Date("02/14/2017"), duration: 2, progress: 100, approved: true },
             { taskID: 11, taskName: "Design complete", startDate: new Date("02/14/2017"), endDate: new Date("02/14/2017"), duration: 0, progress: 0, priority: "Normal", approved: true }
         ]
     },
	 {
	     taskID: 12,
	     taskName: "Implementation Phase",
	     startDate: new Date("02/17/2017"),
	     endDate: new Date("02/27/2017"),
	     priority: "Normal",
	     approved: false,
	     duration: 11,
	     subtasks: [
             {
                 taskID: 13,
                 taskName: "Phase 1",
                 startDate: new Date("02/17/2017"),
                 endDate: new Date("02/27/2017"),
                 priority: "High",
                 approved: false,
                 duration: 11,
                 subtasks: [
                     { taskID: 15, taskName: "Development Task 1", startDate: new Date("02/17/2017"), endDate: new Date("02/19/2017"), duration: 3, progress: "50", priority: "High", approved: false },
                     { taskID: 16, taskName: "Development Task 2", startDate: new Date("02/17/2017"), endDate: new Date("02/19/2017"), duration: 3, progress: "50", priority: "Low", approved: true },
                     { taskID: 17, taskName: "Testing", startDate: new Date("02/20/2017"), endDate: new Date("02/21/2017"), duration: 2, progress: "0", priority: "Normal", approved: true },
                     { taskID: 18, taskName: "Bug fix", startDate: new Date("02/24/2017"), endDate: new Date("02/25/2017"), duration: 2, progress: "0", priority: "Critical", approved: false },
                     { taskID: 19, taskName: "Customer review meeting", startDate: new Date("02/26/2017"), endDate: new Date("02/27/2017"), duration: 2, progress: "0", priority: "High", approved: false },
                     { taskID: 20, taskName: "Phase 1 complete", startDate: new Date("02/27/2017"), endDate: new Date("02/27/2017"), duration: 0, priority: "Low", approved: true }

                     ]
             },

             {
                 taskID: 21,
                 taskName: "Phase 2",
                 startDate: new Date("02/17/2017"),
                 endDate: new Date("02/28/2017"),
                 priority: "High",
                 approved: false,
                 duration: 12,
                 subtasks: [
                     { taskID: 23, taskName: "Development Task 1", startDate: new Date("02/17/2017"), endDate: new Date("02/20/2017"), duration: 4, progress: "50", priority: "Normal", approved: true },
                     { taskID: 24, taskName: "Development Task 2", startDate: new Date("02/17/2017"), endDate: new Date("02/20/2017"), duration: 4, progress: "50", priority: "Critical", approved: true },
                     { taskID: 25, taskName: "Testing", startDate: new Date("02/21/2017"), endDate: new Date("02/24/2017"), duration: 2, progress: "0", priority: "High", approved: false },
                     { taskID: 26, taskName: "Bug fix", startDate: new Date("02/25/2017"), endDate: new Date("02/26/2017"), duration: 2, progress: "0", priority: "Low", approved: false },
                     { taskID: 27, taskName: "Customer review meeting", startDate: new Date("02/27/2017"), endDate: new Date("02/28/2017"), duration: 2, progress: "0", priority: "Critical", approved: true },
                     { taskID: 28, taskName: "Phase 2 complete", startDate: new Date("02/28/2017"), endDate: new Date("02/28/2017"), duration: 0, priority: "Normal", approved: false }

                     ]
             },

             {
                 taskID: 29,
                 taskName: "Phase 3",
                 startDate: new Date("02/17/2017"),
                 endDate: new Date("02/27/2017"),
                 priority: "Normal",
                 approved: false,
                 duration: 11,
                 subtasks: [
                     { taskID: 31, taskName: "Development Task 1", startDate: new Date("02/17/2017"), endDate: new Date("02/19/2017"), duration: 3, progress: "50", priority: "Low", approved: true },
                     { taskID: 32, taskName: "Development Task 2", startDate: new Date("02/17/2017"), endDate: new Date("02/19/2017"), duration: 3, progress: "50", priority: "Normal", approved: false },
                     { taskID: 33, taskName: "Testing", startDate: new Date("02/20/2017"), endDate: new Date("02/21/2017"), duration: 2, progress: "0", priority: "Critical", approved: true },
                     { taskID: 34, taskName: "Bug fix", startDate: new Date("02/24/2017"), endDate: new Date("02/25/2017"), duration: 2, progress: "0", priority: "High", approved: false },
                     { taskID: 35, taskName: "Customer review meeting", startDate: new Date("02/26/2017"), endDate: new Date("02/27/2017"), duration: 2, progress: "0", priority: "Normal", approved: true },
                     { taskID: 36, taskName: "Phase 3 complete", startDate: new Date("02/27/2017"), endDate: new Date("02/27/2017"), duration: 0, priority: "Critical", approved: false }
                     ]
             }
	     ]
	 }
];

var textWrapData = [
     {
         taskID: 1,
         taskName: "Design",
         startDate: new Date("02/10/2017"),
         endDate: new Date("02/14/2017"),
         duration: 3,
         progress: 86,
         priority: "High",
         approved: false,
         subtasks: [
             { taskID: 2, taskName: "Software Specification", startDate: new Date("02/10/2017"), endDate: new Date("02/12/2017"), duration: 3, progress: 60, priority: "Normal", approved: false },
             { taskID: 3, taskName: "Develop prototype", startDate: new Date("02/10/2017"), endDate: new Date("02/12/2017"), duration: 3, progress: 100, priority: "Critical", approved: false },
             { taskID: 4, taskName: "Get approval from customer", startDate: new Date("02/13/2017"), endDate: new Date("02/14/2017"), duration: 2, progress: 100, approved: true },
             { taskID: 5, taskName: "Design Documentation", startDate: new Date("02/13/2017"), endDate: new Date("02/14/2017"), duration: 2, progress: 100, approved: true },
             { taskID: 6, taskName: "Design complete", startDate: new Date("02/14/2017"), endDate: new Date("02/14/2017"), duration: 0, progress: 0, priority: "Normal", approved: true }
         ]
     },
	 {
	     taskID: 7,
	     taskName: "Implementation Phase",
	     startDate: new Date("02/17/2017"),
	     endDate: new Date("02/27/2017"),
	     priority: "Normal",
	     approved: false,
	     duration: 11,
	     subtasks: [
             {
                 taskID: 8,
                 taskName: "Phase 1",
                 startDate: new Date("02/17/2017"),
                 endDate: new Date("02/27/2017"),
                 priority: "High",
                 approved: false,
                 duration: 11,
                 subtasks: [{
                     taskID: 9,
                     taskName: "Implementation Module 1",
                     startDate: new Date("02/17/2017"),
                     endDate: new Date("02/27/2017"),
                     priority: "Normal",
                     duration: 11,
                     approved: false,
                     subtasks: [
                         { taskID: 10, taskName: "Development Task 1", startDate: new Date("02/17/2017"), endDate: new Date("02/19/2017"), duration: 3, progress: "50", priority: "High", approved: false },
                         { taskID: 11, taskName: "Development Task 2", startDate: new Date("02/17/2017"), endDate: new Date("02/19/2017"), duration: 3, progress: "50", priority: "Low", approved: true },
                         { taskID: 12, taskName: "Testing", startDate: new Date("02/20/2017"), endDate: new Date("02/21/2017"), duration: 2, progress: "0", priority: "Normal", approved: true },
                         { taskID: 13, taskName: "Bug fix", startDate: new Date("02/24/2017"), endDate: new Date("02/25/2017"), duration: 2, progress: "0", priority: "Critical", approved: false },
                         { taskID: 14, taskName: "Customer review meeting", startDate: new Date("02/26/2017"), endDate: new Date("02/27/2017"), duration: 2, progress: "0", priority: "High", approved: false },
                         { taskID: 15, taskName: "Phase 1 complete", startDate: new Date("02/27/2017"), endDate: new Date("02/27/2017"), duration: 0, priority: "Low", approved: true }

                     ]
                 }]
             },

             {
                 taskID: 16,
                 taskName: "Phase 2",
                 startDate: new Date("02/17/2017"),
                 endDate: new Date("02/28/2017"),
                 priority: "High",
                 approved: false,
                 duration: 12,
                 subtasks: [{
                     taskID: 17,
                     taskName: "Implementation Module 2",
                     startDate: new Date("02/17/2017"),
                     endDate: new Date("02/28/2017"),
                     priority: "Critical",
                     approved: false,
                     duration: 12,
                     subtasks: [
                         { taskID: 18, taskName: "Development Task 1", startDate: new Date("02/17/2017"), endDate: new Date("02/20/2017"), duration: 4, progress: "50", priority: "Normal", approved: true },
                         { taskID: 19, taskName: "Development Task 2", startDate: new Date("02/17/2017"), endDate: new Date("02/20/2017"), duration: 4, progress: "50", priority: "Critical", approved: true },
                         { taskID: 20, taskName: "Testing", startDate: new Date("02/21/2017"), endDate: new Date("02/24/2017"), duration: 2, progress: "0", priority: "High", approved: false },
                         { taskID: 21, taskName: "Bug fix", startDate: new Date("02/25/2017"), endDate: new Date("02/26/2017"), duration: 2, progress: "0", priority: "Low", approved: false },
                         { taskID: 22, taskName: "Customer review meeting", startDate: new Date("02/27/2017"), endDate: new Date("02/28/2017"), duration: 2, progress: "0", priority: "Critical", approved: true },
                         { taskID: 23, taskName: "Phase 2 complete", startDate: new Date("02/28/2017"), endDate: new Date("02/28/2017"), duration: 0, priority: "Normal", approved: false }

                     ]
                 }]
             },

             {
                 taskID: 24,
                 taskName: "Phase 3",
                 startDate: new Date("02/17/2017"),
                 endDate: new Date("02/27/2017"),
                 priority: "Normal",
                 approved: false,
                 duration: 11,
                 subtasks: [{
                     taskID: 25,
                     taskName: "Implementation Module 3",
                     startDate: new Date("02/17/2017"),
                     endDate: new Date("02/27/2017"),
                     priority: "High",
                     approved: false,
                     duration: 11,
                     subtasks: [
                         { taskID: 26, taskName: "Development Task 1", startDate: new Date("02/17/2017"), endDate: new Date("02/19/2017"), duration: 3, progress: "50", priority: "Low", approved: true },
                         { taskID: 27, taskName: "Development Task 2", startDate: new Date("02/17/2017"), endDate: new Date("02/19/2017"), duration: 3, progress: "50", priority: "Normal", approved: false },
                         { taskID: 28, taskName: "Testing", startDate: new Date("02/20/2017"), endDate: new Date("02/21/2017"), duration: 2, progress: "0", priority: "Critical", approved: true },
                         { taskID: 29, taskName: "Bug fix", startDate: new Date("02/24/2017"), endDate: new Date("02/25/2017"), duration: 2, progress: "0", priority: "High", approved: false },
                         { taskID: 30, taskName: "Customer review meeting", startDate: new Date("02/26/2017"), endDate: new Date("02/27/2017"), duration: 2, progress: "0", priority: "Normal", approved: true },
                         { taskID: 31, taskName: "Phase 3 complete", startDate: new Date("02/27/2017"), endDate: new Date("02/27/2017"), duration: 0, priority: "Critical", approved: false },

                     ]
                 }]
             }
	     ]
	 }
];
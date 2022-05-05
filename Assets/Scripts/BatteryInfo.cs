using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//this should definitely be handled somewhere else and passed here
[Serializable]
public class BatteryForm {
public string ambientTemp;
}

// Currently stores info into your Appdata (if on windows)
// this is just gonna be %user%/appdata/localow/defaultcompany/practicesomething
public class BatteryInfo : MonoBehaviour {

// text input field - should hopefully (in the future) be dynamically assigned to the current text field
// eventually this will just be a list of elements
public Text change;

//Character name string
private string path;

DateTime today = DateTime.Today;

// idea is that pressing the submit button first sends a message out passing the current on screen fields
// need to figure out how to grab each field on screen and pass in a list
// changeInfo or whatever function i need to use then takes these fields and prepares them for storing below
// way more manageable in the long run + waaayyy less bloat
public void changeInfo(Text toChange) {
        change = toChange;
}

public void StoreInfo()
{
        path = Path.Combine(Application.persistentDataPath, String.Format("BatteryForm-{0}-{1}-{2}.json", today.Day, today.Month, today.Year));

        BatteryForm form = new BatteryForm();
        form.ambientTemp = change.text;

        string json = JsonUtility.ToJson(form);

        using (var fileStream = new FileStream(path, FileMode.OpenOrCreate))
                using (StreamWriter streamWriter = new StreamWriter(fileStream)) {
                        streamWriter.WriteLine(json);
                }
}
}


/* jotting down some notes 02/20/19

    - probably smart to keep the input menu separate ? (as it currently is)
        - comes down to how context aware we can make those elements
        - i'd still like to have groupings of some forms to make it more clear
            - e.g. things like positive ground test / negative ground test together
            - this would mean separate input menus based on these groupings
            - pros: keeps it clear + readable
                - otherwise double checking to see if you cleared a form for *every* single item would get tedious
                - (possible to just highlight when something has been submitted via the changing the color of the button)
            - cons: bloat
    - storing new logs
        - based on current session or should the user have control over new forms?
        - should def be able to see the current state of the form
        - regardless, it's probably best to create forms in the format of Battery-dd-mm-yy-#
        - need to modify StoreInfo to do the following
            - check to see if the form for today's date has been created, if not create it
                - alongside current form meaning something needs to keep track of which iteration it is
                - probably have a new / load form button somewhere that keeps track of the #
                - initializing the app probably needs to set the # and keep track of what forms have already been created for today
            - creating a new form should write the needed fields automatically and *this* script handles writing into them
        - user repeating an item should just overwrite what's already there

 */

/* new notes 04/14/19
---------------------------
f: form class
    store -> takes in a name + value
          -> compare value against fields w/ find(name)
          		| or index the buttons somehow to avoid this
          -> if null, uh
          -> if not null, store value into that field
    find -> takes in a name
         -> finds field w/ same string value
         -> passes it back (or null if not found)
    writeForm -> writes to .csv
    		     | specifics come later


s: extensions of form class -> just holds a set of fields per type of inspection

g: class that generates/loads forms
	takes in form type -> generates appropriate form
			           -> passes off to c
	loads in old form  -> i .. am not sure how to handle this just yet
		      	           | initial guess is the first part of the file name should be the type of form so that it knows what to load into
				           | e.g. batterybankinspection-01-01-19.csv or some intermediary .json or whatever
				           | creates a new form and loads the data into the correct fields, then passes to c?

c: class that holds current form
	when accepting a new form       -> generate file associated w/ it
	when receiving new button input -> stores name into "tempName" or something
	when receiving new form input   -> calls (thisform).store(tempName, value)
				      		            | check completion here?
				                    -> call (thisform).checkCompletion to see if true
				                    -> if true, (thisform).writeForm()

battery bank clicked -> new form
		             -> passes "batterybank" to g
		             -> resets button text values to default?

(thinking buttons should list their current values in their text fields... probably)

item selected -> pass the name of the item back to c
	          -> when submit is pressed, pass whatever value was selected/entered to c
	          -> change button's name based on value ? c should probably do this because it knows what button was pressed

future considerations
	uncompleted forms? way of marking completion without editing data? maybe w/ the filename ?
	app suddenly closes? need a listener that saves contents of g on some closing event

handle this GENERICALLY or SPECIFICALLY

generic -> need a way of creating a form w/ fields based on names of buttons associated w/ it
	    -> potentially store that info into a .json file and then have to scan through it when looking for battery bank
    	    | e.g. first creates "field" like "ambienttemp" (name of button) in "batterybankinspection.json" upon starting inspection
    		| then, when Ambient Temperature is clicked, it looks for its name in the file to know that's where it stores whatever is entered
    		| this idea feels very stupid
	    -> in order to make this a reality, i need to know how to handle the creation of fields during runtime. which uhh, is probably not possible

specific -> just create extensions of a default "form" class that have the necessary fields predefined
	     -> c can still be generic enough where it just passes everything back and then checks to see if the form is done
    		| e.g. form.checkCompletion() which looks to see if any fields are still null
    		| doesn't have to care about what it's passing as it assumes the correct type of form has been selected
    		| this idea feels good enough

*/

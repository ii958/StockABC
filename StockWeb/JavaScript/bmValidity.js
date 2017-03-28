// cut off space from head and tail of theString.
			function trim(theString)
			{
				var checkedAll = false;
				var checkString = theString;
				while ((!checkedAll) && (checkString.length != 0))
				{
					// cut off head space
					if (checkString.indexOf(' ') == 0)
					{
						checkString = checkString.substring(1);
						continue;
					}
					
					//  cut off tail space
					if (checkString.lastIndexOf(' ') == (checkString.length - 1))
					{
						checkString = checkString.substring(0,checkString.length - 1);
						continue;
					}
					
					checkedAll = true;
				}
				return checkString;
			}

			// set focus to object refered by argument
			function focus(theObj)
			{
				if (theObj != null)
				{
					if(!theObj.disabled)
					{
						theObj.focus();
					}
				}
			}

			// Judge if the value of theObj is blank, 
			// if is, return true, else return false.
			function isBlank(theObj)
			{
				if (theObj == null)
					return true;
				
				var theValue;
				
				if (theObj.tagName.toLowerCase() == "input")
				  theValue = theObj.value;
				else
				  theValue = theObj.innerText;

				theValue = trim(theValue);
				
				if (theValue == "")
					return true;
				else
					return false;	
			}

			// Judge if the value of theObj is a date string in format YYYY-MM-DD
			// if is, return true else return false
			function isDate(theObj)
			{
				if (theObj == null)
					return false;
					
				var theValue = theObj.value;	
				theValue = Trim(theValue);
				
				var theData = theValue.split("-");	
				if (theData.length != 3)
					return false;
				
				
				// verify number
				if (isNaN(theData[0]) || isNaN(theData[1]) || isNaN(theData[2]))
					return false;
				
				// verify year is vaild
				var year = parseInt(theData[0], 10);
				if (year < 1900 || year > 9999)
					return false;
				
				
				// verify month is valid 
				var month = parseInt(theData[1], 10);
				if (month < 1 || month > 12)
					return false;
				
				// verify day is valid
				var dayLength = 0;		
				switch (month)
				{
					case 2:
						if ((year%4==0)&&((year%100!=0)||(year%400==0)))
						{
							dayLength = 29;
						}
						else
						{
							dayLength = 28;
						}
						break;
					case 1:
					case 3:
					case 5:
					case 7:
					case 8:
					case 10:
					case 12:
						dayLength = 31;
						break;
					case 4:
					case 6:
					case 9:
					case 11:
						dayLength = 30;
						break;
				}
				var day = parseInt(theData[2], 10);
				if (day < 1 || day > dayLength)
					return false;
				
				return true;
			}

			// Judge if the length of the value of theObj is lengther than maxLength
			function isOverLength(theObj, maxLength) 
			{
				if (theObj == null)
					return true;
					
				var theValue = theObj.value;
				theValue = trim(theValue);
							
				if (theValue.length > maxLength)
					return true;
				else
					return false;
			}
			
			function isInt(theObj)
			{ 
				if (isNaN(parseInt(theObj.value)))
					return false;
				else
				{
					if (parseInt(theObj.value).toString() != theObj.value.toString())
						return false;
				}
					
				return true;
			}
			
			function isFloat(theObj)
			{ 
				if (isNaN(parseInt(theObj.value)))
					return false;
				else
				{
					if (parseFloat(theObj.value).toString() != theObj.value.toString())
						return false;
				}
					
				return true;
			}

			// Judge if the value of theObj is a valid email address
			function isEmail(theObj)
			{ 
				if(theObj == null)
					return false;
				
				var theValue = theObj.value;
				var vEmail = theValue;
				var pattern = /^([a-zA-Z0-9_-])+(\.[a-zA-Z0-9_-])*@([a-zA-Z0-9_-])+(\.[a-zA-Z0-9_-])+/;
　　			return pattern.test(vEmail); 　　
　　			} 
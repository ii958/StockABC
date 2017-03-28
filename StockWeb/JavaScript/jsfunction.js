	/*
		?????????????
	*/	
	
	function ShowModelDiag(url,width,height,scrollbars,resizable)
	{
		var feature = "dialogWidth="+width+"px;dialogHeight="+height+"px;center=1;Status=no;toolbar=no;menubar=no;location=no;scrol="+ scrollbars +";resizable="+ resizable; 
		var win = window.showModalDialog(url,"newWindow",feature);
		return win;		
	}
	
	function ShowCenterWindow(url,width,height,scrollbars,resizable)
	{
		var top = (window.screen.availHeight-height)/2;
		var left = (window.screen.availWidth-width)/2;
		var win = window.open(url,"newWindow","width="+width+",height="+height+",top="+top+",left="+left+",Status=yes,toolbar=yes,menubar=yes,location=yes,scrollbars="+ scrollbars +",resizable="+ resizable +"");
		win.focus();
	}

	/*???????м?λ?????*/
	function ShowNoScrollWindow(url,width,height)
	{
		var top = (window.screen.availHeight-height)/2;
		var left = (window.screen.availWidth-width)/2;
		var win = window.open(url,"","width="+width+",height="+height+",top="+top+",left="+left+",Status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no");
		win.focus();
	}
	
	/*
		???????м?λ?????
		?????????????????С

	*/
	function ShowNoScrollResizableWindow(url,width,height,resizable)
	{
		var top = (window.screen.availHeight-height)/2;
		var left = (window.screen.availWidth-width)/2;
		if(resizable == 'yes')
		{
			var win = window.open(url,"","width="+width+",height="+height+",top="+top+",left="+left+",Status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=yes");
			win.focus();
		}
		else
			ShowNoScrollWindow(url,width,height);
	}

	function IsFormatNumber(nValue,maxDecimalPoint,bName)
	{
		
		var reg = new RegExp("(^((-)?[1-9][0-9]{0,16}(\.[0-9]{1,"+maxDecimalPoint+"})?$)|(^(-)?[0](\.[0-9]{1,"+maxDecimalPoint+"})?$))");
	
		if(reg.test(nValue)	== false )
		{
			alert(bName+"必须为浮点数，最多保留"+maxDecimalPoint+"小数位!");
			event.returnValue = false;
			return false;
		}
		else
			return true;
	}	
	
	
	/*
		??????м?λ?????

		?????????
	*/
	function ShowNoScrollOneWindow(url,width,height)
	{
		var top = (window.screen.availHeight-height)/2;
		var left = (window.screen.availWidth-width)/2;
		var win = window.open(url,"JustOneWindow","width="+width+",height="+height+",top="+top+",left="+left+",Status=no,toolbar=no,menubar=no,location=no,scrollbars=no,resizable=no");
		win.focus();
	}


	/*???????м?λ?????*/
	function ShowWindow(url,width,height)
	{
		var top = (window.screen.availHeight-height)/2;
		var left = (window.screen.availWidth-width)/2;
		var win = window.open(url,"","width="+width+",height="+height+",top="+top+",left="+left+",Status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no");
		win.focus();
	}
	
	/*
		???????м?λ?????
	*/
	function ShowOneWindow(url,width,height)
	{
		var top = (window.screen.availHeight-height)/2;
		var left = (window.screen.availWidth-width)/2;
		var win = window.open(url,"JustOneWindow","width="+width+",height="+height+",top="+top+",left="+left+",Status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no");
		win.focus();
	}
	
		
		/*???????м?λ?????*/
	function ShowScrollResizeWindow(url,width,height)
	{
		var top = (window.screen.availHeight-height)/2;
		var left = (window.screen.availWidth-width)/2;
		var win = window.open(url,"","width="+width+",height="+height+",top="+top+",left="+left+",Status=no,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=yes");
		win.focus();
	}
	
	



	/*	LTrim(string):?????????


	==================================================================

	*/

	function LTrim(str)

	{

		var whitespace = new String(" \t\n\r");

		var s = new String(str);

	    

		if (whitespace.indexOf(s.charAt(0)) != -1)

		{

			var j=0, i = s.length;

			while (j < i && whitespace.indexOf(s.charAt(j)) != -1)

			{

				j++;

			}

			s = s.substring(j, i);

		}

		return s;

	}

 

	/*

	==================================================================

	RTrim(string):?????????

	==================================================================

	*/

	function RTrim(str)

	{

		var whitespace = new String(" \t\n\r");

		var s = new String(str);

	 

		if (whitespace.indexOf(s.charAt(s.length-1)) != -1)

		{

			var i = s.length - 1;

			while (i >= 0 && whitespace.indexOf(s.charAt(i)) != -1)

			{

				i--;

			}

			s = s.substring(0, i+1);

		}

		return s;

	}

	 

	/*

	==================================================================

	Trim(string):????????


	==================================================================

	*/

	function Trim(str)
	{

		return RTrim(LTrim(str));

	}


			
	//????????????????

	function CheckDate(theObj)
	{
		if (theObj == null)
			return true;
		if(Trim(theObj.value) == "")
		{
			return true;
		}
		
		var inPutValue = Trim(theObj.value).split(" ");
		 
		var theValue = inPutValue[0];
		
		var theData = theValue.split("-");	
	
		if (theData.length != 3)
		{
			return false;
		}
		
		// Check is number
		if (isNaN(theData[0]) || isNaN(theData[1]) || isNaN(theData[2]))
		{
			return false;
		}
		if(theData[0]=='' || theData[1]=='' || theData[2]=='')
		{
			return false;
		}
		if(theData[0]==null || theData[1]==null || theData[2]==null)
		{
			return false;
		}
		// Check Year
		var year = parseInt(theData[0], 10);
		if (year < 1900 || year > 9999)
		{
			return false;
		}
		
		// Check Month
		var month = parseInt(theData[1], 10);
		if (month < 1 || month > 12)
		{
			return false;
		}

		// Check day
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
		{
			return false;
		}
		
		var theTime ;
		
		var inPutLength = inPutValue.length;
		
		if(inPutLength>1)	
		{
			theTime = inPutValue[inPutLength-1].split(":");
		
			if (isNaN(theTime[0]) || isNaN(theTime[1]) || isNaN(theTime[2]))
			{
				return false;
			}
			if(theTime[0]=='' || theTime[1]=='' || theTime[2]=='')
			{
				return false;
			}
			if(theTime[0]==null || theTime[1]==null || theTime[2]==null)
			{
			
				return false;
			}
			
			var h = parseInt(theTime[0], 10);
			
			if(h<0 || h>=24)
			{
				return false;
			}
			
			var m =  parseInt(theTime[1], 10);
			if(m<0 || m>=60)
			{
				return false;
			}
			
			var s =  parseInt(theTime[2], 10);
			if(s <0 || s>=60)
			{
				return false;
			}
			
		}
		return true;
	} 
	
	function CheckDateValue(theObjValue)
	{

		if(Trim(theObjValue) == "")
		{
			return false;
		}
		
		var inPutValue = Trim(theObjValue).split(" ");
		 
		var theValue = inPutValue[0];
		
		var theData = theValue.split("-");	
	
		if (theData.length != 3)
		{
			return false;
		}
		
		// Check is number
		if (isNaN(theData[0]) || isNaN(theData[1]) || isNaN(theData[2]))
		{
			return false;
		}
		if(theData[0]=='' || theData[1]=='' || theData[2]=='')
		{
			return false;
		}
		if(theData[0]==null || theData[1]==null || theData[2]==null)
		{
			return false;
		}
		// Check Year
		var year = parseInt(theData[0], 10);
		if (year < 1900 || year > 9999)
		{
			return false;
		}
		
		// Check Month
		var month = parseInt(theData[1], 10);
		if (month < 1 || month > 12)
		{
			return false;
		}

		// Check day
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
		{
			return false;
		}
		
		var theTime ;
		
		var inPutLength = inPutValue.length;
		
		if(inPutLength>1)	
		{
			theTime = inPutValue[inPutLength-1].split(":");
		
			if (isNaN(theTime[0]) || isNaN(theTime[1]) || isNaN(theTime[2]))
			{
				return false;
			}
			if(theTime[0]=='' || theTime[1]=='' || theTime[2]=='')
			{
				return false;
			}
			if(theTime[0]==null || theTime[1]==null || theTime[2]==null)
			{
			
				return false;
			}
			
			var h = parseInt(theTime[0], 10);
			
			if(h<0 || h>=24)
			{
				return false;
			}
			
			var m =  parseInt(theTime[1], 10);
			if(m<0 || m>=60)
			{
				return false;
			}
			
			var s =  parseInt(theTime[2], 10);
			if(s <0 || s>=60)
			{
				return false;
			}
			
		}
		return true;
	} 
	
//??ж????????????

function isMoney(pObj,errMsg)
{
	var obj=eval(pObj);
	strRef="1234567890.-";
	if(!isEmpty(pObj,errMsg))return false;
	for(i=0;i<obj.value.length;i++)
	{
		tempChar=obj.value.substring(i,i+1);
		if (strRef.indexOf(tempChar,0)==-1)
		{
			if (errMsg==null||errMsg=="")
				alert("?????????,????");
			else
				alert(errMsg);
			if(obj.type=="text")
				obj.focus();
			return false;
		}
		else
		{
			tempLen=obj.value.indexOf(".");
			if(tempLen!=-1)
			{
				strLen=obj.value.substring(tempLen+1,obj.value.length);
				if(strLen.length>2)
				{
					if(errMsg==null||errMsg=="")
						alert("??????????,????");
					else
						alert(errMsg);
					if(obj.type=="text")
						obj.focus();
					return false;
				}
			}
		}
	}
	return true;
}

/*
???????????
?? onKeypress ??????

???????? 0123456789.-
*/
function OnlyNumber()
{
	if(!
	(((window.event.keyCode >= 48) && (window.event.keyCode <= 57))
	|| (window.event.keyCode == 13) 
	|| (window.event.keyCode == 46) 
	|| (window.event.keyCode == 45))
	)
	{
		window.event.keyCode = 0;
	}
}

/*

IsInt(string,string,int or string):(??????,+ or - or empty,empty or 0)

?????ж????????????????????????+0????+0

*/

function IsInt(objStr,sign,zero)

{

    var reg;    

    var bolzero;    

    

    if(Trim(objStr)=="")

    {

        return false;

    }

    else

    {

        objStr=objStr.toString();

    }    

    

    if((sign==null)||(Trim(sign)==""))

    {

        sign="+-";

    }

    

    if((zero==null)||(Trim(zero)==""))

    {

        bolzero=false;

    }

    else

    {

        zero=zero.toString();

        if(zero=="0")

        {

            bolzero=true;

        }

        else

        {

            alert("??????0????????(???0)");

        }

    }

    

    switch(sign)

    {

        case "+-":

            //???


            reg=/(^-?|^\+?)\d+$/;            

            break;

        case "+": 

            if(!bolzero)           

            {

                //??????

                reg=/^\+?[0-9]*[1-9][0-9]*$/;

            }

            else

            {

                //??????+0

                //reg=/^\+?\d+$/;

                reg=/^\+?[0-9]*[0-9][0-9]*$/;

            }

            break;

        case "-":

            if(!bolzero)

            {

                //????

                reg=/^-[0-9]*[1-9][0-9]*$/;

            }

            else

            {

                //????+0

                //reg=/^-\d+$/;

                reg=/^-[0-9]*[0-9][0-9]*$/;

            }            

            break;

        default:

            alert("????????????(???+??-)");

            return false;

            break;

    }

    

    var r=objStr.match(reg);

    if(r==null)

    {

        return false;

    }

    else

    {        

        return true;     

    }

}

function formatCurrencyNoComma(num) 
{
	num = num.toString().replace(/\$|\,/g,'');
	if(isNaN(num))
	num = "0";
	sign = (num == (num = Math.abs(num)));
	num = Math.floor(num*100+0.50000000001);
	cents = num%100;
	num = Math.floor(num/100).toString();
	if(cents<10)
	cents = "0" + cents;
	for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
	{
		num = num.substring(0,num.length-(4*i+3))+num.substring(num.length-(4*i+3));
	}
	
	return (((sign)?'':'-') + num + '.' + cents);
}

function formatCurrency(num) 
{
	num = num.toString().replace(/\$|\,/g,'');
	if(isNaN(num))
	num = "0";
	sign = (num == (num = Math.abs(num)));
	num = Math.floor(num*100+0.50000000001);
	cents = num%100;
	num = Math.floor(num/100).toString();
	if(cents<10)
	cents = "0" + cents;
	for (var i = 0; i < Math.floor((num.length-(1+i))/3); i++)
	num = num.substring(0,num.length-(4*i+3))+','+
	num.substring(num.length-(4*i+3));
	return (((sign)?'':'-') + num + '.' + cents);
}

//判断字符串长度；
// 中文字符计算为两个长度

function StrLen(str)
{
    var strlength=0;
    for (var i=0;i<str.length;i++)
    {
        if(str.charCodeAt(i)>=1000)
            strlength += 2;
        else
            strlength += 1;
    }

    return strlength ;
}

// 1、控件对象，根据对象可以取出值

// 2、是否进行空值检查

// 3、控件标识

// 4、允许输入的最大长度

// 返回值为TRUE或FALSE　
// True 表示数据检查合格

// False 表示数据为空值或太大了

function CheckData()
{
	var	obj			= arguments[0] ;
	var canEmpty	= arguments[1] ;
	var	cc			= arguments[2] ;
	var	MaxLength	= arguments[3] ;
	var checkLength	= arguments[4] ;
	
	//alert("obj="+obj+"canEmpty = "+canEmpty +" cc="+cc+"MaxLength="+MaxLength+"checkLength="+checkLength);

	if ( obj == null )
	{
		alert("引用对象为Null！");
		return false ;
	}
	if ( Trim(obj.value) == "")
	{
		if ( canEmpty )
		{
			cc		=	cc	+	"不能为空，请输入！" ;
			alert(cc);
			obj.focus();
			return false  ;
		}
	}
	
	// 申请原因不能小于20个字符
	if ( checkLength != null )
	{
		if ( obj.value.length < 20 )
		{
			cc	=	cc + "不能少于20个字符！";
			alert(cc);
			obj.focus();
			return ;
		}
	}
		
	if ( MaxLength != null )
	{
		if ( StrLen(obj.value) > MaxLength )
		{
			cc		=	cc	+	"不能超过" + MaxLength + "个字符！" ;
			alert(cc);
			obj.focus();
			return false  ;
		}
	}
	
	return true ;
}


/*
检查一个字符串的长度是否符合需求
1、字符串
2、字符串的最大长度
3、提示信息
*/
function CheckDataLength()
{
	var	value		= arguments[0] ;
	var	MaxLength	= arguments[1] ;
	var	Message		= arguments[2] ;	
	
	if ( value == null
		|| MaxLength == null 
		|| Message == null )
	{
			return true ;
	}
	
	if ( StrLen(value) > MaxLength )
	{
		Message	=	Message	+	"长度不能大于" + MaxLength + "个字符！" ;
		alert(Message)
		return false ;
	}

	return true ;
}
function CheckMoneyLength(strMoney,length,msg)
{
	var message = "";
	if(strMoney == null || strMoney.length == 0 || strMoney == "")
	{
		return true;
	}
	var pattern = "";
	if(length == null || length == 0)
	{
		pattern = "(^[0-9]([\.][0-9]{1,2}){0,1}$)|(^[1-9][0-9]{0,}([\.][0-9]{1,2}){0,1}$)";
	}
	else
	{
		pattern = "(^[0-9]([\.][0-9]{1,2}){0,1}$)|(^[1-9][0-9]{0," + (length - 3) + "}([\.][0-9]{1,2}){0,1}$)";
	}
	var re = new RegExp(pattern);
	if(!re.test(strMoney))
	{
		if(length == null || length == 0)
		{
			message = msg + "不正确，只能输入数字，小数点后不能超过2位！";
		}
		else
		{
			message = msg + "不正确，只能输入数字，整数部分不能超过" + (length - 2) + "位数字，小数点后不能超过2位！";
		}
		alert(message);
		return false;
	}
	return true;
}


    function CompareBig(strDateBegin,strDateEnd)
	{
		var strSeparator = "-"; 
		var strDateArrayBegin;
		var strDateArrayEnd;
		var intMonthBegin;
		var intMonthEnd;
		var intYearBegin;
		var intYearEnd;
		var intDayBegin;
		var intDayEnd;
		strDateArrayBegin=strDateBegin.split(strSeparator);
		strDateArrayEnd=strDateEnd.split(strSeparator);
		
		intYearBegin = parseInt(strDateArrayBegin[0],10);
		intMonthBegin = parseInt(strDateArrayBegin[1],10);
		intDayBegin = parseInt(strDateArrayBegin[2],10);
		
		intYearEnd = parseInt(strDateArrayEnd[0],10);
		intMonthEnd = parseInt(strDateArrayEnd[1],10);
		intDayEnd = parseInt(strDateArrayEnd[2],10);
		if(intYearBegin>intYearEnd)
		{
			return false;
		}
		if((intYearBegin==intYearEnd) && (intMonthBegin>intMonthEnd))
		{
			return false;
		}
		if((intYearBegin==intYearEnd) && (intMonthBegin==intMonthEnd) && (intDayBegin>intDayEnd))
		{
			return false
		}
		return true;
	}
	
		function isdate(strDate)
		{
				var strSeparator = "-"; 
				var strDateArray;
				var intYear;
				var intMonth;
				var intDay;
				var boolLeapYear;
				   
				strDateArray = strDate.split(strSeparator);
				   
				if(strDateArray.length!=3) return false;
				 if (isNaN(strDateArray[0]) || isNaN(strDateArray[1]) || isNaN(strDateArray[2]))
				{
					return false;
				}
				intYear = parseInt(strDateArray[0],10);
				intMonth = parseInt(strDateArray[1],10);
				intDay = parseInt(strDateArray[2],10);
				   
				if(isNaN(intYear)||isNaN(intMonth)||isNaN(intDay)) return false;
				if (intYear < 1900 || intYear > 9999)
				{
					return false;
				}
				if(intMonth>12||intMonth<1) return false;
				   
				if((intMonth==1||intMonth==3||intMonth==5||intMonth==7||intMonth==8||intMonth==10||intMonth==12)&&(intDay>31||intDay<1)) return false;
				   
				if((intMonth==4||intMonth==6||intMonth==9||intMonth==11)&&(intDay>30||intDay<1)) return false;
				   
				if(intMonth==2){
					if(intDay<1) return false;
				      
					boolLeapYear = false;
					if((intYear%100)==0){
						if((intYear%400)==0) boolLeapYear = true;
					}
					else{
						if((intYear%4)==0) boolLeapYear = true;
					}
				      
					if(boolLeapYear){
						if(intDay>29) return false;
					}
					else{
						if(intDay>28) return false;
					}
				}
			   
				return true;
			}
			
function disableAllButton()
 {
	var links = window.document.getElementsByTagName("A");
	
	if(links)
	{
		for(var i = 0; i < links.length;i ++)
		{
			if(links[i].id != null && (links[i].id.indexOf("_LinkButtonAction") != -1 || links[i].id.indexOf("_HyperLinkAction") != -1))
				links[i].disabled = true;
		}
	}
 }
 
 function checkEventValid()
 {
	if(event.srcElement != null && event.srcElement.disabled == true)
		return false;
	else
		return true;
 }	
 /*
检查一个字符串的长度是否符合需求
1、字符串
2、字符串的最大长度
3、提示信息
*/
function CheckDataLengthCH()
{
	var	value		= arguments[0] ;
	var	MaxLength	= arguments[1] ;
	var	Message		= arguments[2] ;

	if ( value == null || value.length == 0 || value == ""
		|| MaxLength == null 
		|| Message == null )
	{
		return true ;
	}
	if ( StrLen(value) > MaxLength * 2 )
	{
		Message	=	Message	+	"长度不能大于" + MaxLength + "个中文字符！" ;
		alert(Message)
		return false ;
	}

	return true ;
}	
//遮罩效应
function SetShutOut(imgUrl)
{
	var shut = window.document.getElementById("shutout");
	if(shut)
	{
		shut.style.display = "block";
	}
	else
	{
		var innerHTML = "<div class='bg' id='shutout'><iframe id ='iframeDivID' class='iframeDiv'></iframe></div>"
		var content = "<div id ='content' style='z-index:201'><img src='"+ imgUrl +"'/></div>";
		window.document.body.insertAdjacentHTML("beforeEnd",innerHTML);
		window.document.body.insertAdjacentHTML("beforeEnd",content);
		window.document.body.style.position="relative";
		
		var shut = window.document.getElementById("shutout");
		var w = window.document.body.offsetWidth > window.document.body.scrollWidth ? window.document.body.offsetWidth : window.document.body.scrollWidth;
		var h = window.document.body.offsetHeight > window.document.body.scrollHeight ? window.document.body.offsetHeight : window.document.body.scrollHeight;
		shut.style.width = w;
		shut.style.height = h;
		
		var iframeDiv = window.document.getElementById("iframeDivID");
		iframeDiv.style.width = w;
		iframeDiv.style.height = h;
		
		var content =  window.document.getElementById("content");
		content.style.position = "absolute";
		content.style.left = window.document.body.offsetWidth/2;
		content.style.top = window.document.body.offsetHeight/2;
	}		
}
//去除遮罩
function HiddenShutOut()
{
	var shut = window.document.getElementById("shutout");
	
	if(shut)
		shut.style.display = "none";
}
//添加Cookie
function addCookie(c_name, value, expiredays) 
{
	if (expiredays > 0) 
	{
		var exdate = new Date();
		exdate.setDate(exdate.getDate() + expiredays);
		document.cookie = c_name + "=" + escape(value) + ((expiredays == null) ? "" : "; expires=" + exdate.toUTCString());
	}
}

//取得Cookie值
function getCookie(name)
{ 					
	var strCookie=document.cookie; 
	var arrCookie=strCookie.split("; "); 
	for(var i=0;i<arrCookie.length;i++){ 
		var arr=arrCookie[i].split("="); 
		if(arr[0]==name)return arr[1]; 
	} 
	return ""; 
} 

//通过设置Cookie的过期时间已删除Cookie
function deleteCookie(name)
{ 
	var date=new Date(); 
	date.setTime(date.getTime()-10000); 
	document.cookie=name+"=v; expire="+date.toGMTString(); 
}

//检查浏览器是否允许使用Cookie
function NavigatorCookieEnable()
{ 
	var result=false; 
	if(navigator.cookiesEnabled) return true; 
	document.cookie = "aissetestcookie=yes;"; 
	var cookieSet = document.cookie; 
	if (cookieSet.indexOf("aissetestcookie=yes") > -1) result=true; 
	document.cookie = "";
	return result; 
} 
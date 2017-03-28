var rowSpace = 80,colSpace = 100,divWidth = 100,divHeight = 40;
var maxCol,maxRow;
　
function init(list)
{
    maxCol = getMaxCol(list);
    maxRow = getMaxRow(list);
    
    createAllNode(list);
    linkAllDiv(list);
    showAllCorner();
}

function getMaxRightEdge()
{
    var allDiv = window.document.getElementsByTagName("div");    
    var maxRightEdge = 0;
    var divRightEdge = 0;

    for (var i = 0; i < allDiv.length; i ++)
    {
       divRightEdge = allDiv[i].style.pixelLeft + allDiv[i].style.pixelWidth;

       if (maxRightEdge < divRightEdge)
         maxRightEdge = divRightEdge;
    }

    return maxRightEdge;
}

function createAllNode(list)
{
   for (var i = 0; i < list.length; i ++)
   {
       createDivByNode(list[i], list);
   }
}

//对反向的箭头排序　把距离短的放在前面
function sortReverseLines(line1, line2)
{
	return (Math.abs(line1.fromNode.id.substr(0,2) - line1.toNode.id.substr(0,2)) 
	    - Math.abs(line2.fromNode.id.substr(0,2) - line2.toNode.id.substr(0,2)));
}

function drawReverseLines(reverseLines, list)
{
    reverseLines = reverseLines.sort(sortReverseLines);
 
    for (var i = 0; i < reverseLines.length; i ++)
    {
        linkDiv(window.document.getElementById(reverseLines[i].fromNode.id),
            window.document.getElementById(reverseLines[i].toNode.id),
            getLineColor(reverseLines[i].toNode),
            true,
            true);
    }
}
  
function linkAllDiv(list,maxRightEdge)
{
  var parents = [];
  var reverseLines = [];

  for(var i = 0; i < list.length; i ++)
  {
      parents = getParentNodes(list[i], list);

      for (var j = 0; j < parents.length; j ++)
      {
          //反向的线.或者跨层  通过比较层次可判断
          if (parseFloat(parents[j].id.substr(0,2)) > parseFloat(list[i].id.substr(0,2))
          ||　Math.abs(parseFloat(parents[j].id.substr(0,2)) - parseFloat(list[i].id.substr(0,2))) > 1)
          {
              var line = new Object();
              line.fromNode = parents[j];
              line.toNode = list[i];
  
              reverseLines.push(line);
          }
          else
          {   //正向 直接画即可
              linkDiv(window.document.getElementById(parents[j].id),
                      window.document.getElementById(list[i].id),
                      getLineColor(list[i]),
                      true);
          }
      }
      
      parents = [];
  }
  
  //反向的线或者是跨层的
  drawReverseLines(reverseLines);
}

//----
function getRootNode(list)
{
  var parentId = "";

  for (var i = 0; i < list.length;i ++)
  {
      parentId = list[i].id.substr(4,list[i].id.length - 4);

      if (parentId == "" || parentId == list[i].id.substr(0,4))
          return list[i];
  }
}

function getRowIndex(node)
{
  return parseFloat(node.id.substr(0,2));
}

function getColIndex(node)
{
  return parseFloat(node.id.substr(2,2));
}

function getParentNodes(node,list)
{
  var parentId = node.id.substr(4,node.id.length - 4);
  var parents = [];

  for (var i = 0; i < parentId.length / 4; i ++)
  {
      for (var j = 0; j < list.length; j ++)
      {
          if (list[j].id.substr(0,4) == parentId.substr(i*4,4))
              parents.push(list[j]);
      }
  }
  
  return parents;
}

function getMaxRow(list)
{
  var maxRow = 0;
  var rowIndex = 0;

  for (var i = 0; i < list.length;i ++)
  {
      rowIndex = getRowIndex(list[i]);

      if (maxRow < rowIndex)
          maxRow = rowIndex;
  }
  
  return maxRow;
}

function getMaxCol(list)
{ 
  var maxCol = 0;
  var colIndex = 0;

  for (var i = 0; i < list.length;i ++)
  {
      colIndex = getColIndex(list[i]);

      if (maxCol < colIndex)
          maxCol = colIndex;
  }
  
  return maxCol;
}

function getColorByStatus(status)
{
    var color ;
    
    switch(parseInt(status))
    {
       case 0 : //未开始
        color = "white";
        break;
       case 10 :    //不适用
       color = "gray";
        break;
       case 20 :    //正在处理
       color = "blue";
        break;
       case 30 :    //退回修改
       color = "orange";
        break;
       case 40 :    //等待
       color = "yellow";
        break;
       case 50 :    //审批通过
       color = "green";
        break;
       case 60 :    //审批拒绝
       color = "red";
        break;
       default:
        break;
    }
    
    return color;
}

function getLineColor(toNode)
{
    if (toNode.status == "0" || toNode.status == "10")
        return "gray";
    else
        return "blue";
}

function createLine(x1,y1,x2,y2,color,isFromDownToUp,withArrow,isBlend)
{   
    if (isBlend == null)
        isBlend = false;

      var htm;

      if (isFromDownToUp == true || isBlend == true)
      {
            var x = getMaxRightEdge() + 7; 

            createLine(x1,y1,x,y1,color);
            createLine(x,y1,x,y2,color);
            createLine(x2,y2,x,y2,color);
            
            if (isBlend == true)
                createArrow(x2,y2,"left",color);
            else
                createArrow(x1,y1,"left",color);
      }
      else
      {
          if (x1==x2) //竖
          {
              htm = "<div style='width:2px;font-size:1pt;position: absolute; top: " 
                  + y1 +"px; left: "+ x1 +"px;height:"+ Math.abs(y2-y1) +"px;background-color:"+color+"'></div>";

              window.document.body.appendChild(window.document.createElement(htm));
              
              if (withArrow == true)
                  createArrow(x2,y2,"down",color);
          }
          else if(y1==y2)//横
          {
              htm = "<div style='height:2px;font-size:1pt;position: absolute; top: "
                 + y1 +"px; left: "+ Math.min(x1,x2) +"px;width:"+ (Math.abs(x1-x2) + 2) +"px;background-color:"+color+"'></div>";

              window.document.body.appendChild(window.document.createElement(htm));
              
              if (withArrow == true)
                  createArrow(x2,y2,"right",color);
          }
          else//折
          {   
             var x = parseInt(Math.abs(x1+x2)/2);
             var y = parseInt(Math.abs(y1+y2)/2);
       
             createLine(x1,y1,x1,y,color);
             createLine(x1,y,x2,y,color);
             createLine(x2,y,x2,y2,color);
             createArrow(x2,y2,"down",color);
          }
      }
  }

function createArrow(x,y,direct, color)
{
    var htm;
    
    direct = direct.toLowerCase();
      
    switch(direct)
    {
        case "up":
            htm = "<div style='z-index:-1;font-size:1pt;position: absolute; top:" + y + ";left:" + x 
             + ";border-top:5px solid #FFFFFF;border-right:5px solid #FFFFFF;border-bottom:5px solid "+color+";border-left:5px solid #FFFFFF;'/>";
            break;
        case "down":
             x = x - 3;
             y = y - 5;
             
             htm = "<div style='z-index:-1;font-size:1pt;position: absolute; top:" + y + ";left:" + x 
                + ";border-top:5px solid "+color+";border-right:5px solid #FFFFFF;border-bottom:5px solid #FFFFFF;border-left:5px solid #FFFFFF;'/>";
            break;
        case "left":
             x = x - 5;
             y = y - 6;
             
             htm = "<div style='z-index:-1;font-size:1pt;position: absolute; top:" + y + ";left:" + x 
             + ";border-top:5px solid #FFFFFF;border-right:5px solid "+color+";border-bottom:5px solid #FFFFFF;border-left:5px solid #FFFFFF;'/>";
            break;
        case "right":
             htm = "<div style='z-index:-1;font-size:1pt;position: absolute; top:" + y + ";left:" + x 
             + ";border-top:5px solid #FFFFFF;border-right:5px solid #FFFFFF;border-bottom:5px solid #FFFFFF;border-left:5px solid "+color+";'/>";
            break;
        default:
            break;
    } 
    
    window.document.body.appendChild(window.document.createElement(htm));
}

//按照箭头顺序赋值才可以..isBend:是否强制弯曲
function linkDiv(div1,div2,color,withArrow,isBend)
{
    if (isBend == null)
        isBend = false;

    if (div1 && div2)
    {
        var isFromDownToUp = false;
        var x1,x2,y1,y2;
        
        if (div1.style.pixelTop > div2.style.pixelTop)
        {
            var d =div1;
            
            div1 = div2;
            div2 = d;
            isFromDownToUp = true;
        }
        
       var height1 = div1.style.pixelHeight == 0 ? div1.offsetHeight : div1.style.pixelHeight;
       var height2 = div2.style.pixelHeight == 0 ? div2.offsetHeight : div2.style.pixelHeight;
  
        
        //从下向上画 都连接到右侧
        if(isFromDownToUp == true || isBend == true)
        {
            x1 = div1.style.pixelLeft + parseInt(div1.style.pixelWidth);
            y1 = div1.style.pixelTop + parseInt(height1/2);
            
            x2 = div2.style.pixelLeft + parseInt(div2.style.pixelWidth);
            y2 = div2.style.pixelTop + parseInt(height2/2);
        }
        else
        {
            x1 = div1.style.pixelLeft + parseInt(div1.style.pixelWidth /2);
            y1 = div1.style.pixelTop + parseInt(height1);
            
            x2 = div2.style.pixelLeft + parseInt(div2.style.pixelWidth /2);
            y2 = div2.style.pixelTop;
        }

        createLine(x1,y1,x2,y2,color,isFromDownToUp || isBend,withArrow,isBend);
    }
}

function createDiv(id,left,top,color,text,title,isStartOrEnd,showCheckBox)
{
    if (isStartOrEnd == null)
        isStartOrEnd = false;

    var div = window.document.getElementById(id);

    if (div == null)
    {
       var checkbox = null;
       
       if (showCheckBox == "1")
       {
          checkbox = window.document.createElement("<input id='checkbox" + id + "' type='checkbox'/>");
       }
       
       var span = window.document.createElement("<span>");
       span.innerText = text;
       
       if (isStartOrEnd == true)
       {
           div = window.document.createElement( "<div id='"+id+"' style='background-color: "+ color+ ";width:"
                + divWidth+"px;position:absolute;top:"+top+"px;left:"+left+"px;font-size:9pt'>");

           var topB= window.document.createElement("<b class='rtop' ></b>");
           
           topB.appendChild(window.document.createElement("<b class='r1' style='background-color:"+color+"'></b>"));
           topB.appendChild(window.document.createElement("<b class='r2' style='background-color:"+color+"'></b>"));
           topB.appendChild(window.document.createElement("<b class='r3' style='background-color:"+color+"'></b>"));
           topB.appendChild(window.document.createElement("<b class='r4' style='background-color:"+color+"'></b>"));
           topB.appendChild(window.document.createElement("<b class='r5' style='background-color:"+color+"'></b>"));
           topB.appendChild(window.document.createElement("<b class='r6' style='background-color:"+color+"'></b>"));
           topB.appendChild(window.document.createElement("<b class='r7' style='background-color:"+color+"'></b>"));
           topB.appendChild(window.document.createElement("<b class='r8' style='background-color:"+color+"'></b>"));

           var bottomB = window.document.createElement( "<b class='rbottom'  ></b>");
           
           bottomB.appendChild(window.document.createElement( "<b class='r8' style='background-color:"+color +"'></b>"));
           bottomB.appendChild(window.document.createElement( "<b class='r7' style='background-color:"+color +"'></b>"));
           bottomB.appendChild(window.document.createElement( "<b class='r6' style='background-color:"+color +"'></b>"));
           bottomB.appendChild(window.document.createElement( "<b class='r5' style='background-color:"+color +"'></b>"));
           bottomB.appendChild(window.document.createElement( "<b class='r4' style='background-color:"+color +"'></b>"));
           bottomB.appendChild(window.document.createElement( "<b class='r3' style='background-color:"+color +"'></b>"));
           bottomB.appendChild(window.document.createElement( "<b class='r2' style='background-color:"+color +"'></b>"));
           bottomB.appendChild(window.document.createElement( "<b class='r1' style='background-color:"+"red" +"'></b>")); 
           
           var content  = window.document.createElement(
           "<div style='width:100%;border-left:solid 1px black;border-right:solid 1px black;text-align:center;vertical-align:middle' title='"+title+"'></div>");
                      
           if (checkbox != null)
                content.appendChild(checkbox);
            
           content.appendChild(span);
            
           div.appendChild(topB); 
           div.appendChild(content);
           div.appendChild(bottomB); 
       }
       else
       {
            div = window.document.createElement("<div id='"+id+"' style='border: solid 1px black; background-color: "+ color 
                 + "; width: "+divWidth+"px;height:"+divHeight+"px;position: absolute; top: "+top+"px; left: "+left+"px;font-size:9pt;text-align:center;vertical-align:middle' "
                 + (title == null ? "" : "title=" + title ) + ">");

             if (checkbox != null)
                div.appendChild(checkbox);
  
            div.appendChild(span);
       }

        window.document.body.appendChild(div);
    }
}

function createDivByNode(node,list)
{            
   var positionRow,positionCol;
 
   positionRow = (getRowIndex(node) - 0.5) * rowSpace;
    
   var colIndex = getColIndex(node);

   positionCol = parseInt((maxCol * colSpace + maxCol * divWidth) * colIndex / (getSameRowNodeCount(node,list) + 1) );
    
   createDiv(node.id, 
       positionCol, 
       positionRow,
       getColorByStatus(node.status), 
       node.text,
       node.title,
       getRowIndex(node) == 1 || getRowIndex(node) == getMaxRow(list),
       node.showCheckBox);
}

function getSameRowNodeCount(node, list)
{
    var count  = 0;

    for (var i = 0; i < list.length; i ++)
    {
        if (list[i].id.substr(0,2) == node.id.substr(0,2))
            count += 1;
    }
    
    return count;
}

//弧线 line1是横线
function createArc(line1,line2)
{
    var x,y,color;
    
    x = line2.style.pixelLeft - 4;
    y = line1.style.pixelTop - 5;
    
    splitLine(line1,x);
    
    color = line1.style.backgroundColor;

    var arc = window.document.createElement("<img src='../Images/flowchat_"
        + color + ".gif' style='position:absolute;left:" + x + "px;top:" + y + "px'/>");

    window.document.body.appendChild(arc);
}

//将线在指定位置分成两部分 中间间隔10像素
function splitLine(line, x)
{ 
    var div = window.document.createElement(
        "<div style='height:2px;font-size:1pt;position: absolute; top: "
        + line.style.pixelTop +"px; left: "+(x + 7) +"px;width:"
        + (line.style.pixelLeft + line.style.pixelWidth - x - 7) +"px;background-color:"+line.style.backgroundColor+"'></div>");
 
    window.document.body.appendChild(div);
    
    line.style.width = x - line.style.pixelLeft + 3;
}

//循环检查交点
function showAllCorner()
{
    var allDivs = window.document.getElementsByTagName("div");
    var lines = [];
    
    for (var i = 0; i < allDivs.length; i ++)
    {
       if (allDivs[i].style.pixelWidth == 2 || allDivs[i].style.pixelHeight == 2)
         lines.push(allDivs[i]);
    }
 
    for (var i = 0; i < lines.length; i ++)
    {
       for(var j = 0; j < lines.length; j ++)
       {
         if (checkHaveCorner(lines[i],lines[j]))
         {
            createArc(lines[i],lines[j])
           
          }
       }
    }
}

//判断两线是否有交点 注意只在line是横线时有效
function checkHaveCorner(line1, line2)
{ 
    //平行线或者重合线
    if (line1.style.pixelWidth == line2.style.pixelWidth 
        || line1.style.pixelHeight == line2.style.pixelHeight)
        return false;
    
    //横线
    if (line1.style.pixelHeight == 2)
    {  //line1  的纵坐标在line2的两端之间。　　　line2的横坐标在line1的两端之间
        if (line1.style.pixelLeft + line1.style.pixelWidth > line2.style.pixelLeft
            && line1.style.pixelLeft < line2.style.pixelLeft
            && line2.style.pixelTop + line2.style.pixelHeight > line1.style.pixelTop
            && line2.style.pixelTop < line1.style.pixelTop)
            { 
                return true;
            }
    }
     
    return false;
}

function getSelectedIds()
{
    var checkboxs = window.document.getElementsByTagName("input");
    var selectedIds = [];
    
    for (var i = 0; i <  checkboxs.length; i ++)
    {
        if (checkboxs[i].type == "checkbox" && checkboxs[i].checked == true)
            selectedIds.push(checkboxs[i].id.replace("checkbox",""));
    }
    
    return selectedIds;
}
function ClearSessionTimeout(sessionTimer){
    clearTimeout(sessionTimer);
    SetSessionTimeout();
};

function SetSessionTimeout(){
    var TimeOut = new Date();
    TimeOut.setTime(TimeOut.getTime() + 1800000); //30 minutes =1800000 milliseconds
    sessionTimer = setTimeout('SessionTimeout(' + TimeOut.getTime() + ')', 1800000);//29 minutes =1740000 milliseconds
   
};

function SessionTimeout(TimeOut){
    /*
    var TimeoutDate = new Date();
    TimeoutDate.setTime(TimeOut);
    */
    
    alert('Your login session has expired.\n Please login again.');
    document.location = 'Logout.aspx';
    
    /*
    var Answer = confirm('Your session will time out at ' + TimeoutDate.toLocaleTimeString() + '. Click OK to continue your session, or Cancel to Log Off.');
    var tDate=new Date();
    if (Answer==true){
        SessionTimeoutKeepAlive();
        
        if (new Date() < TimeoutDate){
            SessionTimeoutKeepAlive();
        }else{
            document.location = 'Logout.aspx';
        };
        
    }else{
        document.location = 'Logout.aspx';
    }*/
    
    
};

function SessionTimeoutKeepAlive(){
    keepSessionAlive('', '');
};


<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IntelWebSite.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>IntelWeb System</title>
    <link href="inc/MasterStyles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div id="TopWraper">
	        <div class="TopCenDiv">
    	        <div class="Logo"></div>
            </div>
        </div>
        <div class="MidBlock">
            <div class="MidCenDiv">
                <div class="LoginBoxDiv LeftGap15 TopGap15">
                    <div class="LoginDiv">
                        <div class="Row">
                            <div class="FieldName">Email :</div>
                            <div class="FieldValue"><input type="text" class="TextBox" /></div>
                        </div>
                        <div class="Row">
                            <div class="FieldName">Password :</div>
                            <div class="FieldValue"><input type="password" class="TextBox" /></div>
                        </div>
                        <div class="Row">
                	        <div class="FieldName"></div>
                            <div class="FieldValue">
                    	        <input type="checkbox" class="chkBox"/> <span class="R">Remeber Me</span>
                    	         <div class="ButtonsDiv">
                                    <a href="WebHome.aspx" class="Button">Sign in</a>
                                </div>
                            </div>
                        </div>
                        <div class="ForGotDiv">
                            <a href="#">Forgot password?</a> | <a href="#">Help </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="FooterDiv">
	        <div class="FooterCenDiv">
    	        <div class="FootNote">&copy; 2011 - Intelweb System - Terms &amp; Conditions</div>
            </div>
        </div>
    </form>
</body>
</html>

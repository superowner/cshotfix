/*
* LCL support c# hotfix here.
*Copyright(C) LCL.All rights reserved.
* URL:https://github.com/qq576067421/cshotfix 
*QQ:576067421 
* QQ Group: 673735733 
 * Licensed under the GNU License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at 
* http://fsf.org/ 
* Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License. 
*/  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class LCLFunctionDelegate
{
     public delegate void method_delegate0(System.Object arg0);
     public delegate void method_delegate1(System.Object arg0,System.Int32 arg1);
     public delegate System.Single[] method_delegate2(System.Object arg0,ref System.Int32 arg1,out System.Single arg2);
     public delegate LCL.DataClass method_delegate3(System.Object arg0,LCL.DataClass arg1);
}
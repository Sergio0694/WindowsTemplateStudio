﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Localization.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class HelpMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal HelpMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Localization.Resources.HelpMessages", typeof(HelpMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Extract localizable items for different cultures.
        ///
        ///Localization ext -o &quot;original_WTS_folder&quot; -a &quot;actual_WTS_folder&quot; -d &quot;destinationDirectory&quot;
        ///
        ///        original_WTS_folder      - path to the folder that contains
        ///                                   old version of WTS to compare
        ///
        ///        actual_WTS_folder        - path to the folder that contains
        ///                                   actual version of WTS to compare
        ///
        ///        destinationDirectory     - path to the folder in which will be
        ///                 [rest of string was truncated]&quot;;.
        /// </summary>
        public static string ExtCommand {
            get {
                return ResourceManager.GetString("ExtCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Generate the missing localized files for different cultures in the templates.
        ///
        ///Localization gen -s &quot;sourceDirectory&quot;
        ///
        ///        sourceDirectory          - path to the folder that contains
        ///                                   source files for Templates
        ///                                   (it&apos;s root project folder).
        ///
        ///Example:
        ///
        ///        Localization gen -s &quot;C:\\MyFolder\\wts&quot;.
        /// </summary>
        public static string GenCommand {
            get {
                return ResourceManager.GetString("GenCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Provides Help information for Windows Template Studio Localization Tool.
        ///
        ///Localization help [command]
        ///
        ///        command - displays help information on that command..
        /// </summary>
        public static string HelpCommand {
            get {
                return ResourceManager.GetString("HelpCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to For more information on a specific command, type HELP command-name
        ///
        ///EXT        Extract localizable items for different cultures.
        ///GEN        Generates Project Templates for different cultures.
        ///MERGE      Merge localizable items to projects and templates.
        ///VERIFY     Verify if exist localizable items for different cultures.
        ///HELP       Provides Help information for Windows Template Studio Localization Tool..
        /// </summary>
        public static string Info {
            get {
                return ResourceManager.GetString("Info", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Merge localizable items to projects and templates.
        ///
        ///Localization merge -s &quot;sourceDirectory&quot; -d &quot;destinationDirectory&quot;
        ///
        ///        sourceDirectory          - path to the folder that contains
        ///                                   localizable items to merge.
        ///
        ///        destinationDirectory     - path to the folder that contains
        ///                                   source files for Project Templates
        ///                                   (it&apos;s root project folder).
        ///
        ///Example:
        ///
        ///        Localization merge -s &quot;C:\\M [rest of string was truncated]&quot;;.
        /// </summary>
        public static string MergeCommand {
            get {
                return ResourceManager.GetString("MergeCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Command unknown..
        /// </summary>
        public static string UnknownCommand {
            get {
                return ResourceManager.GetString("UnknownCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Verify if exist localizable items for different cultures.
        ///
        ///Localization verify -s &quot;sourceDirectory&quot;
        ///
        ///        sourceDirectory          - path to the folder that contains
        ///                                   source files for verify
        ///                                   (it&apos;s root project folder).
        ///
        ///Example:
        ///
        ///        Localization verify -s &quot;C:\\MyFolder\\wts&quot;.
        /// </summary>
        public static string VerifyCommand {
            get {
                return ResourceManager.GetString("VerifyCommand", resourceCulture);
            }
        }
    }
}

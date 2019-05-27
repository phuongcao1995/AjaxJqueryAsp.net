@model IEnumerable<Plexus.L4.Web.Models.VideosViewModel>

@{
    ViewBag.Title = "Videos";
    var userId = @ViewBag.Id;
}

@if (Request.IsAuthenticated)
{
    @*@Styles.Render("~/Content/video-js.css")*@
    @*@Styles.Render("//cdnjs.cloudflare.com/ajax/libs/video.js/5.7.1/video-js.min.css")*@
    @*@Styles.Render("//cdnjs.cloudflare.com/ajax/libs/qtip2/2.2.1/jquery.qtip.css")*@

    @*@Styles.Render("//amp.azure.net/libs/amp/1.6.2/skins/amp-default/azuremediaplayer.min.css");*@
    @*@Styles.Render("~/Content/azuremediaplayer.min.css")*@

    @*@Styles.Render("//amp.azure.net/libs/amp/2.1.5/skins/amp-default/azuremediaplayer.min.css");*@
    @*@Scripts.Render("//amp.azure.net/libs/amp/2.1.5/azuremediaplayer.min.js")*@

    @Styles.Render("~/Content/azuremediaplayer.min.css");
    @Scripts.Render("~/Scripts/azuremediaplayer.min.js")

    <div ng-app="MetaDataApp">

        <div ng-controller="MetaDataSearchController">
            <div id="quick_search">
                <div class="quick_search_cont">
                    <div class="quick_search_field">
                        <input type="text" placeholder="@( User.IsInRole("District Attorney") || User.IsInRole("DA Admin")? "Search case numbers" : "Search comments and tags")" class="searchfield" id="quicksearchfield" name="quicksearchfield" ng-keyup="quicksearch($event)">
                    </div>
                </div>
                <div class="seach-panel">
                    <div class="basic_search_label">
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary" ng-click="quicksearch();"><span class="glyphicon glyphicon-search"></span>&nbsp;Search</button>
                            <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown">
                                <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu">
                                <li><a ng-click="clearSearch()">Clear Search</a></li>
                            </ul>
                        </div>
                    </div>
                    @if (!User.IsInRole("District Attorney") && !User.IsInRole("DA Admin"))
                    {
                        <div class="advanced_search_label">
                            <a href="#" id="ladvsearch">ADVANCED SEARCH</a>
                        </div>
                    }
                </div>
                <div>
                    @if (!User.IsInRole("District Attorney") && !User.IsInRole("DA Admin"))
                    {
                        <div class="preset_name">
                            <div class="btn-group">
                                <button type="button" class="btn btn-primary" data-toggle="dropdown">Show/Hide</button>
                                <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown">
                                    <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu" role="menu">
                                    @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin"))
                                    {
                                        <li><a class="toggleDeletedVideos" ng-click="toggleDeletedVideos()">Show Deleted</a></li>
                                    }
                                    <li><a class="toggleDashCamVideos" ng-click="toggleDashCamVideos()">Show Dash Cam</a></li>
                                    <li><a class="toggleBodyCamVideoStartEndTime" ng-click="toggleBodyCamVideoStartEndTime()">Show Start Time</a></li>
                                </ul>
                            </div>
                        </div>
                    }

                </div>
                <div class="clear"></div>
            </div>

            <div id="advanced_search">
                <div class="searchfields">
                    <div class="s1 sfield" ng-cloak>
                        <div class="border-div">
                            <div class="search-field-header">
                                <span>RECORDED</span> <span class="glyphicon glyphicon-info-sign" id="recordedDateSign"></span>
                            </div>
                            <div class="search-field-content">
                                <input type="text" placeholder="From" id="RecordedDateFrom" name="RecordedDateFrom" class="form-control" />
                                <div class="horizontalSpace"></div>
                                <input type="text" placeholder="To" id="RecordedDateTo" name="RecordedDateTo" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <div class="s2 sfield" ng-cloak>
                        <div class="border-div">
                            <div class="search-field-header">
                                <span>UPLOADED</span> <span class="glyphicon glyphicon-info-sign" id="uploadedDateSign"></span>
                            </div>
                            <div class="search-field-content">
                                <input type="text" placeholder="From" id="UploadedFrom" name="UploadedFrom" class="form-control" />
                                <div class="horizontalSpace"></div>
                                <input type="text" placeholder="To" id="UploadedTo" name="UploadedTo" class="form-control" />
                            </div>
                        </div>
                    </div>
                    <div class="s3 sfield" ng-cloak>
                        <div class="border-div">
                            <div class="search-field-header">DURATION</div>
                            <div class="search-field-content">
                                <div id="DurationFrom" placeholder="Min" name="DurationFrom" class="duration-time form-control" style="font-family:'Open Sans',arial;font-size:11px;"></div>
                                <div class="horizontalSpace"></div>
                                <div id="DurationTo" placeholder="Max" name="DurationTo" class="duration-time form-control" style="font-family:'Open Sans',arial;font-size:11px;"></div>
                            </div>
                        </div>
                    </div>
                    <div class="s4 sfield" ng-cloak>
                        <div class="border-div">
                            <div class="search-field-header">OFFICER</div>
                            <div>
                                <select ng-model="searchMetadata.Officer" class="form-control">
                                    <option value="">[Any Officer]</option>
                                    <option value="[No Officer]">[No Officer]</option>
                                    <option ng-repeat="option in officers" value="{{option}}">{{option}}</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="s5 sfield" ng-cloak>
                        <div class="border-div">
                            <div class="search-field-header">TYPE</div>
                            <div>
                                <select ng-model="searchMetadata.EncounterType" class="form-control">
                                    <option value="">[Any Type]</option>
                                    <option ng-repeat="option in types" value="{{option}}">{{option}}</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="s5 sfield" ng-cloak>
                        <div class="border-div">
                            <div class="search-field-header">CASE #</div>
                            <div><input type="text" placeholder="Enter #" ng-model="searchMetadata.CaseNumber" id="CaseNumber" name="CaseNumber" class="form-control" /></div>
                        </div>
                    </div>
                    <div class="s5 sfield" ng-cloak>
                        <div class="border-div">
                            <div class="search-field-header">TITLE</div>
                            <div><input type="text" placeholder="Enter Title" ng-model="searchMetadata.Title" id="Title" name="Title" class="form-control" /></div>
                        </div>
                    </div>
                    <div class="s6 sfield" ng-cloak>
                        <div class="border-div">
                            <div class="search-field-header">TAGS</div>
                            <div><input type="text" placeholder="Enter Tag" ng-model="searchMetadata.Tags" id="Tags" name="Tags" class="form-control" /></div>
                        </div>
                    </div>
                    <div class="s7 sfield" ng-cloak>
                        <div class="border-div">
                            <div class="search-field-header">COMMENTS</div>
                            <div><input type="text" placeholder="Enter Keyword" ng-model="searchMetadata.Comments" id="Comments" name="Comments" class="form-control" /></div>
                        </div>
                    </div>
                    <div class="s8 sfield" ng-cloak>
                        <div class="border-div">
                            <div class="search-field-header">STATUS</div>
                            <div>
                                <select ng-model="searchMetadata.Status" class="form-control">
                                    <option value="">[Any Status]</option>
                                    <option ng-repeat="option in statuses" value="{{option}}">{{option}}</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="s9 sfield" ng-cloak>
                        <div class="border-div">
                            <div class="search-field-header">LOCKED</div>
                            <div><input type="checkbox" ng-model="searchMetadata.Locked" id="Locked" name="Locked" class="search-checkbox form-control" /></div>
                        </div>
                    </div>
                    <div class="vehicle-dropdown s10 sfield hideDOM" ng-cloak>
                        <div class="border-div">
                            <div class="search-field-header"><span>VEHICLE</span></div>
                            <div>
                                <select ng-model="searchMetadata.VehicleType" class="form-control">
                                    <option value=""></option>
                                    <option value="0">&lt;Undefined&gt;</option>
                                    <option ng-repeat="v in $root.vehicleTypes" value="{{v.VehicleId}}">{{v.VehicleType}}</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin") || User.IsInRole("Supervisor"))
                    {
                        <div class="s12 sfield" ng-cloak>
                            <div class="border-div">
                                <div class="search-field-header"><span>GROUP</span></div>
                                <div>
                                    <select ng-model="searchMetadata.GroupId" class="form-control">
                                        <option value="">[Any Group]</option>
                                        <option value="0">[No Group]</option>
                                        <option ng-repeat="(Id, Name) in $root.groups" value="{{Id}}">{{Name}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        if (User.IsInRole("Global Admin"))
                        {
                            <div class="s11 sfield" ng-cloak>
                                <div class="border-div">
                                    <div class="search-field-header"><span>ORGANIZATION</span></div>
                                    <div>
                                        <select ng-model="searchMetadata.Organization" class="form-control" ng-change="hideDownloadVideos(searchMetadata.Organization)">
                                            <option value="">[Any Organization]</option>
                                            <option value="0">[No Organization]</option>
                                            <option ng-repeat="(OrganizationId, Name) in organizations" value="{{OrganizationId}}">{{Name}}</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        }
                    }
                </div>
                <div class="clear"></div>
                <div style="padding: 0 6px 6px 6px; margin-top: 28px">
                    @if (!User.IsInRole("District Attorney") && !User.IsInRole("DA Admin"))
                    {
                        <div class="btn-group pull-left">
                            <button type="button" class="btn btn-primary" data-toggle="dropdown">Show/Hide</button>
                            <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown">
                                <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu">
                                @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin"))
                                {
                                    <li><a class="toggleDeletedVideos" ng-click="toggleDeletedVideos()">Show Deleted</a></li>
                                }
                                <li><a class="toggleDashCamVideos" ng-click="toggleDashCamVideos()">Show Dash Cam</a></li>
                                <li><a class="toggleBodyCamVideoStartEndTime" ng-click="toggleBodyCamVideoStartEndTime()">Show Start Time</a></li>
                            </ul>
                        </div>
                    }
                    <div class="searchbtn">
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary" id="advancedSearch" ng-click="search();"><span class="glyphicon glyphicon-search"></span>&nbsp;Search</button>
                            <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown">
                                <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu">
                                <li><a ng-click="clearSearch()">Clear Search</a></li>
                            </ul>
                        </div>
                        <br />
                        <br />
                        <a href="#" id="lqsearch">BASIC SEARCH</a>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </div>
        <div ng-controller="MetaDataController">
            <input type="hidden" id="previousUrl" value="@ViewBag.ReturnUrl" />
            <input type="hidden" id="cookieExpirationTime" value="@ViewBag.CookieExpirationTime" />
            <div id="vid_entries_gallery">
                <div class="entry_container" ng-cloak>
                    @*<span class="glyphicon glyphicon-trash recycle-bin" ng-cloak ng-show="isRecycleBin"></span>
                        <span class="glyphicon glyphicon-user my-videos" ng-cloak ng-show="isMyVideo"></span>*@
                    <div id="left-entry-container">
                        <div class="results_label" ng-cloak>
                            SHOWING <br class="br-class" /> <span> {{ totalResults }}<span ng-show="groupByView">/{{totalVideos}}</span> RESULTS</span> <br />PAGE {{pageNumber}} / {{ totalPages }}
                            <span class="glyphicon glyphicon-info-sign" data-toggle="modal" data-target="#infoModal"></span>
                        </div>
                    </div>
                    <div id="center-entry-container">
                        <div class="preset-input-container">
                            @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin"))
                            {
                                <div class="preset_input" ng-hide="$root.isHideDownloadVideos || allVideoMetadata.length == 0">
                                    <div class="wrap-preset-input">
                                        <button class="btn btn-primary custom-button" style="" ng-click="DeleteVideos(videoMetadata,'#shareAllVideosModal')" ng-cloak>Share All Videos</button>
                                    </div>
                                </div>
                                if (User.IsInRole("Global Admin"))
                                {
                                    <div class="preset_input" ng-hide="$root.isHideDownloadVideos || allVideoMetadata.length == 0">
                                        <div class="wrap-preset-input">
                                            <button class="btn btn-primary custom-button" style="" ng-click="DeleteVideos(videoMetadata,'#deleteVideosModal')" ng-cloak>Delete All Videos</button>
                                        </div>
                                    </div>
                                }
                            }
                            <div class="preset_input">
                                <div class="wrap-preset-input">
                                    <button class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important; float: left; margin-right: 2px" type="button" id="btn_multipleVideos" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" ng-click="multipleVideos()">
                                        <span><i class="state-icon glyphicon" ng-class="isEnabledMultiVideos === true ? 'glyphicon-check' : 'glyphicon-unchecked'"></i></span> Select Multiple
                                    </button>
                                    <a href="javascript:void(0)" ng-click="listSelectedVideos.length > 0 ? playMultiVideos(): return;" ng-disabled="listSelectedVideos.length === 0" style="font-size: 22px;float: left;color: white;margin-top: -2px;">
                                        <i class="fa fa-play-circle-o" ng-class="listSelectedVideos.length === 0 ? 'fa-disabled' : ''" aria-hidden="true" ng-cloak></i>
                                    </a>
                                </div>
                            </div>
                            @if (!User.IsInRole("District Attorney") && !User.IsInRole("DA Admin"))
                            {
                                <div class="preset_input">
                                    <div class="wrap-preset-input dropdown page-size pull-left">
                                        <button class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;" type="button" id="dropdownMenu3" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                            <span class="group-by-button-text">Group By</span>
                                            <span class="caret"></span>
                                        </button>
                                        <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
                                            @if (User.IsInRole("Global Admin"))
                                            {
                                                <li ng-repeat="n in ['None', 'Encounter Type', 'Organization'] track by $index">
                                                    <a ng-click="groupBy(n)">{{ n }}</a>
                                                </li>
                                            }
                                            else if (User.IsInRole("Organization Admin")
                                                || User.IsInRole("Officer V2")
                                                || User.IsInRole("View entire department")
                                                || User.IsInRole("Supervisor"))
                                            {
                                                <li ng-repeat="n in ['None', 'Encounter Type', 'Officer'] track by $index">
                                                    <a ng-click="groupBy(n)">{{ n }}</a>
                                                </li>
                                            }
                                            else if (User.IsInRole("Officer")
                                                || User.IsInRole("Corrections"))
                                            {
                                                <li ng-repeat="n in ['None', 'Encounter Type'] track by $index">
                                                    <a ng-click="groupBy(n)">{{ n }}</a>
                                                </li>
                                            }
                                            @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin") || User.IsInRole("Supervisor"))
                                            {
                                                <li>
                                                    <a ng-click="groupBy('Group')">Group</a>
                                                </li>
                                            }
                                        </ul>
                                    </div>
                                </div>
                            }
                            <div class="preset_input recordedby_preset hideDOM">

                                <div class="wrap-preset-input dropdown page-size pull-left">
                                    <button class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;" type="button" id="dropdownMenu4" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" ng-show="groupByView == false">
                                        <span class="recorded-by-button-text">Recorded By</span>
                                        <span class="caret"></span>
                                    </button>
                                    <div class="tooltip-wrapper" title="Sort By is unavailable in this view" ng-cloak ng-show="groupByView == true">
                                        <button class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;" type="button" id="dropdownMenu4" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" ng-disabled="groupByView == true">
                                            <span class="recorded-by-button-text">Recorded By</span>
                                            <span class="caret"></span>
                                        </button>
                                    </div>
                                    <ul class="dropdown-menu" aria-labelledby="dropdownMenu3">
                                        <li>
                                            <a ng-click="filterByDevice(-1)">All</a>
                                        </li>
                                        <li>
                                            <a ng-click="filterByDevice(1)">BWC</a>
                                        </li>
                                        <li>
                                            <a ng-click="filterByDevice(2)">Dash Cam</a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="preset_input">

                                <div class="wrap-preset-input dropdown page-size pull-left">
                                    <button class="btn btn-default dropdown-toggle " style="padding: 0px 10px !important;" type="button" id="dropdownMenu2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" ng-show="groupByView == false">
                                        <span class="sort-by-button-text">Sort By</span>
                                        <span class="caret"></span>
                                    </button>
                                    <div class="tooltip-wrapper" title="Sort By is unavailable in this view" ng-cloak ng-show="groupByView == true">
                                        <button class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;" type="button" id="dropdownMenu2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" ng-disabled="groupByView == true">
                                            <span class="sort-by-button-text">Sort By</span>
                                            <span class="caret"></span>
                                        </button>
                                    </div>
                                    <ul class="dropdown-menu" aria-labelledby="dropdownMenu2">
                                        <li>
                                            <a ng-click="order('recorded')">Recorded</a>
                                        </li>
                                        <li>
                                            <a ng-click="order('uploaded')">Uploaded</a>
                                        </li>
                                        <li>
                                            <a ng-click="order('duration')">Duration</a>
                                        </li>
                                        <li>
                                            <a ng-click="order('officer')">Officer</a>
                                        </li>
                                        <li>
                                            <a ng-click="order('type')">Encounter Type</a>
                                        </li>
                                        <li>
                                            <a ng-click="order('caseNumber')">Case #</a>
                                        </li>
                                        <li>
                                            <a ng-click="order('title')">Title</a>
                                        </li>
                                        <li>
                                            <a ng-click="order('tags')">Tags</a>
                                        </li>
                                        <li>
                                            <a ng-click="order('comments')">Comments</a>
                                        </li>
                                        <!-- <li>
                                            <a ng-click="order('status')">Status</a>
                                        </li> -->
                                        <li>
                                            <a ng-click="order('isLocked')">Locked</a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="preset_input">
                                <div class="wrap-preset-input dropdown page-size pull-left">
                                    <button class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" ng-show="groupByView == false">
                                        <span class="show-dropdown-text">Show</span>
                                        <span class="caret"></span>
                                    </button>
                                    <div class="tooltip-wrapper" title="Pagination is unavailable in this view" ng-cloak ng-show="groupByView == true">
                                        <button class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" ng-disabled="groupByView == true">
                                            <span class="show-dropdown-text">Show</span>
                                            <span class="caret"></span>
                                        </button>
                                    </div>
                                    <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
                                        <li ng-repeat="n in [10,25,50,100] track by $index">
                                            <a ng-click="pageSizeChange(n)">{{ n }}</a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="preset_input select-page-dropdown">
                                <div class="wrap-preset-input btn-group">
                                    <button type="button" class="btn btn-default select-page-button" style="padding: 0px 10px !important;" ng-click="getItems(0)" ng-disabled="allVideoMetadata.length == 0" ng-show="groupByView == false">Select Page</button>
                                    <button type="button" class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;margin-left: -3px" data-toggle="dropdown" ng-disabled="allVideoMetadata.length == 0" ng-show="groupByView == false">
                                        <span class="caret"></span>
                                    </button>
                                    <div class="tooltip-wrapper" title="Pagination is unavailable in this view" ng-cloak ng-show="groupByView == true">
                                        <button type="button" class="btn btn-default select-page-button" style="padding: 0px 10px !important;" ng-click="getItems(0)" ng-disabled="groupByView == true">Select Page</button>
                                        <button type="button" class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;margin-left: -3px" data-toggle="dropdown" ng-disabled="groupByView == true">
                                            <span class="caret"></span>
                                        </button>
                                    </div>
                                    <ul class="dropdown-menu" role="menu">
                                        <li ng-repeat="n in getTotalPages() track by $index">
                                            <a ng-click="getItems($index + 1)">{{$index + 1}}</a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="right-entry-container">
                        <div class="presets">
                            <div id="igaactive" class="view_mode_icon"><img src="~/Images/icon_gallery_active.png"></div>
                            <div id="ilinactive" class="view_mode_icon"><a href="#" class="llist"><img src="~/Images/icon_list_inactive.png"></a></div>
                        </div>
                        <div class="preset_label">
                            MODE
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
                <div style="padding-left:18px;padding-top:18px;padding-right:18px;">
                    <div class="vid_entry" ng-hide="isLoading || groupByView == true" id="{{ videoMetadata.VideoMetadataId }}_videoMetadata" ng-repeat="videoMetadata in allVideoMetadata" ngc-done ng-cloak>
                        <span class="vid_thumb"><img src="@ViewBag.thumbNailUrl{{videoMetadata.Video.FileName}}.jpg" alt=" "></span>
                        <span class="vid_play" ng-click="setVideo(videoMetadata)" ng-show="isEnabledMultiVideos === false"><a href="javascript:void(0)" id="vid_thumbnail" title="Play Video" onclick="loadvid()"><img src="~/Images/fpo_play.png"></a></span>
                        <span class="vid_play" ng-hide="isHideCheckbox(videoMetadata.VideoMetadataId)" style="vertical-align: middle;top: 50%;left: 50%;margin-top: -20px;" ng-show=""><input type="checkbox" class="styled-checkbox" id="styled-checkbox-{{videoMetadata.VideoMetadataId}}" ng-checked="listSelectedVideos.indexOf(videoMetadata.VideoMetadataId) !== -1" ng-click="selectVideo(videoMetadata.VideoMetadataId, listSelectedVideos.indexOf(videoMetadata.VideoMetadataId))" /><label for="styled-checkbox-{{videoMetadata.VideoMetadataId}}"></label></span>
                        <span class="vid_meta"><span class="vid_timestamp">{{ GetRecordedDate (videoMetadata) }}</span><span class="vid_duration">{{ videoMetadata.Video.Duration | duration:'seconds' }}</span></span>

                        <span class="vid_btns">
                            <a href="#" title="Video Details" style="cursor: pointer" ng-click="ShowDetails(videoMetadata,'#detailModal')"><img src="~/Images/icon_list.png" class="vid_icon item_ico_edit action"></a>

                            @if (!User.IsInRole("View Only") && !User.IsInRole("District Attorney") && !User.IsInRole("DA Admin"))
                            {
                                if (User.IsInRole("Officer V2")
                                    || (User.IsInRole("View entire department") && !User.IsInRole("Supervisor") && !User.IsInRole("Organization Admin") && !User.IsInRole("Global Admin")))
                                {
                                    <a href="#" title="Edit Video Details" ng-if="videoMetadata.Video.UserId == '@userId'" ng-click="EditItem(videoMetadata,'#editModal')"><img src="~/Images/icon_edit.png" class="vid_icon action" style="cursor: pointer"></a>
                                }
                                else
                                {
                                    <a href="#" title="Edit Video Details" ng-click="EditItem(videoMetadata,'#editModal')"><img src="~/Images/icon_edit.png" class="vid_icon action" style="cursor: pointer"></a>
                                }

                            }
                            <img src="~/Images/icon_alert.png" title="Locked" class="vid_icon action" ng-if="videoMetadata.Video.PreventDeletion == true">

                            @if (User.IsInRole("Global Admin")
|| User.IsInRole("Organization Admin")
|| User.IsInRole("Downloader")
|| User.IsInRole("District Attorney")
|| User.IsInRole("DA Admin")
|| User.IsInRole("Download"))
                            {

                                // DOWNLOAD BUTTON
                                <a class="download-movie" title="Download Video" ng-click="downloadVideo(videoMetadata,'#downloadModal')" download>
                                    @*href="/Data/Download/{{videoMetadata.Video.VideoId}{" download="/Data/Download/{{videoMetadata.Video.VideoId}}">*@
                                    <img src="~/Images/icon_download.png" class="item_ico_download action" style="cursor: pointer">
                                </a>
                            }

                            @if (User.IsInRole("Global Admin")
|| User.IsInRole("Organization Admin")
|| User.IsInRole("Share"))
                            {
                                // SHARE BUTTON
                                <a href="javascript:void(0)" title="Share Video" ng-click="ShareVideos(videoMetadata,'#emailModal')">
                                    <i class="fa fa-share-square-o action" aria-hidden="true" style="color: #BEBEC0; font-size: 26px;"></i>
                                </a>
                            }

                            @if (User.IsInRole("Global Admin")
|| User.IsInRole("Organization Admin")
|| User.IsInRole("Redact"))
                            {
                                // REDACT BUTTON
                                <a href="javascript:void(0)" title="Redact Video" ng-click="ShareVideos(videoMetadata,'#redactModal')">
                                    <img src="~/Images/icon_redact.png" style="cursor: pointer" class="action" />
                                </a>
                            }
                        </span>
                    </div>

                    <div ng-show="!isLoading && groupByView == true" ng-repeat="(key, value) in allVideoMetadata | groupBy: groupByType" ngc-done ng-cloak>
                        <div class="group_section">
                            <span class="group_title">{{ groupByType === "GroupId" ? getGroupNameById(key, value[0].OrganizationId) : getGroupTitle(key) }}</span>
                            <span class="group_link">[<a href="#" title="See All" ng-click="seeAllvideos(key)">See All</a>]</span>
                        </div>
                        <div class="vid_entry" ng-hide="isLoading" id="{{ videoMetadata.VideoMetadataId }}_videoMetadata" ng-repeat="videoMetadata in value | limitTo:5">
                            <span class="vid_thumb"><img src="@ViewBag.thumbNailUrl{{videoMetadata.Video.FileName}}.jpg" alt=" "></span>
                            <span class="vid_play" ng-click="setVideo(videoMetadata)" ng-show="isEnabledMultiVideos === false"><a href="javascript:void(0)" id="vid_thumbnail" title="Play Video" onclick="loadvid()"><img src="~/Images/fpo_play.png"></a></span>
                            <span class="vid_play" ng-hide="isHideCheckbox(videoMetadata.VideoMetadataId)" style="vertical-align: middle;top: 50%;left: 50%;margin-top: -20px;" ng-show=""><input type="checkbox" class="styled-checkbox" id="group-by-styled-checkbox-{{videoMetadata.VideoMetadataId}}" ng-checked="listSelectedVideos.indexOf(videoMetadata.VideoMetadataId) !== -1" ng-click="selectVideo(videoMetadata.VideoMetadataId, listSelectedVideos.indexOf(videoMetadata.VideoMetadataId))" /><label for="group-by-styled-checkbox-{{videoMetadata.VideoMetadataId}}"></label></span>
                            <span class="vid_meta"><span class="vid_timestamp">{{ GetRecordedDate (videoMetadata) }}</span><span class="vid_duration">{{ videoMetadata.Video.Duration | duration:'seconds' }}</span></span>

                            <span class="vid_btns">
                                <a href="#" title="Video Details" style="cursor: pointer" ng-click="ShowDetails(videoMetadata,'#detailModal')"><img src="~/Images/icon_list.png" class="vid_icon item_ico_edit action"></a>

                                @if (!User.IsInRole("View Only"))
                                {
                                    if (User.IsInRole("Officer V2")
                                        || (User.IsInRole("View entire department") && !User.IsInRole("Supervisor") && !User.IsInRole("Organization Admin") && !User.IsInRole("Global Admin")))
                                    {
                                        <a href="#" title="Edit Video Details" ng-if="videoMetadata.Video.UserId == '@userId'" ng-click="EditItem(videoMetadata,'#editModal')"><img src="~/Images/icon_edit.png" class="vid_icon action" style="cursor: pointer"></a>
                                    }
                                    else
                                    {
                                        <a href="#" title="Edit Video Details" ng-click="EditItem(videoMetadata,'#editModal')"><img src="~/Images/icon_edit.png" class="vid_icon action" style="cursor: pointer"></a>
                                    }
                                }
                                <img src="~/Images/icon_alert.png" title="Locked" class="vid_icon action" ng-if="videoMetadata.Video.PreventDeletion == true">

                                @if (User.IsInRole("Global Admin")
|| User.IsInRole("Organization Admin")
|| User.IsInRole("District Attorney")
|| User.IsInRole("DA Admin")
|| User.IsInRole("Downloader")
|| User.IsInRole("Download"))
                                {

                                    // DOWNLOAD BUTTON
                                    <a class="download-movie" title="Download Video" ng-click="downloadVideo(videoMetadata, '#downloadModal')" download>
                                        <img src="~/Images/icon_download.png" class="item_ico_download action" style="cursor: pointer">
                                    </a>
                                }

                                @if (User.IsInRole("Global Admin")
|| User.IsInRole("Organization Admin")
|| User.IsInRole("Share"))
                                {

                                    // SHARE BUTTON

                                <a href="javascript:void(0)" title="Share Video" ng-click="ShareVideos(videoMetadata,'#emailModal')">
                                    <i class="fa fa-share-square-o action" aria-hidden="true" style="color: #BEBEC0; font-size: 26px;"></i>
                                </a>
                                }
                                @if (User.IsInRole("Global Admin")
|| User.IsInRole("Organization Admin")
|| User.IsInRole("Redact"))
                                {

                                    // REDACT BUTTON
                                    <a href="javascript:void(0)" title="Redact Video" ng-click="ShareVideos(videoMetadata,'#redactModal')">
                                        <img src="~/Images/icon_redact.png" style="cursor: pointer" class="action" />
                                    </a>
                                }
                            </span>
                        </div>
                    </div>
                </div>

                <div ng-show="allVideoMetadata.length == 0 && !isLoading" ng-cloak style="padding: 80px;">
                    <div class="text-center">No Videos Found</div>
                </div>

                <div ng-show="isLoading">
                    <div style="padding-top:100px;padding-bottom:100px;text-align:center;color:#6f6f6f;">Loading...<br><br><img src="/Images/loading.gif" class="center-block" /></div>
                </div>

                <div class="clear"></div>
                <div class="paginationl4" ng-cloak ng-show="!isLoading">
                    <a href="javascript:void(0)" onmousedown="return false" ng-class="{ disabled: pageNumber == 1 }" aria-label="Previous" ng-click="first()" ng-disabled="pageNumber == 1 || groupByView == true"><span aria-hidden="true"><< FIRST</span></a> &nbsp;&nbsp; <a href="javascript:void(0)" onmousedown="return false" aria-label="Previous" ng-click="previous()" ng-class="{ disabled: pageNumber == 1 }" ng-disabled="pageNumber == 1 || groupByView == true"><span aria-hidden="true">< PREV</span></a> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <a href="javascript:void(0)" onmousedown="return false" aria-label="Next" ng-click="next()" ng-class="{ disabled: pageNumber == (totalPages) || totalPages == 0 }" ng-disabled="pageNumber == (totalPages) || groupByView == true || totalPages == 0"><span aria-hidden="true">NEXT ></span></a> &nbsp;&nbsp; <a href="javascript:void(0)" onmousedown="return false" aria-label="Last" ng-click="last()" ng-class="{ disabled: pageNumber == (totalPages) || totalPages == 0}" ng-disabled="pageNumber == (totalPages) || groupByView == true || totalPages == 0"><span aria-hidden="true">LAST >></span></a>
                </div>



            </div>
            <div class="listToShareVideos">
                @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin"))
                {
                    <div id="shareAllVideosModal" class="modal fade" tabindex="-1" role="dialog" ng-form="shareAllVideosForm">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h4 class="modal-title">SHARE ALL VIDEOS</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="row" style="width: 99%;margin: auto;margin-top: 8px;">
                                        <input type="email" id="organizationEmail" class="form-control" name="organizationEmail" placeholder="Please enter organization's email" ng-model="organizationEmail" required />
                                    </div>
                                    <div class="row" style="width: 99%;margin: auto;margin-top: 8px;">
                                        <input type="checkbox" id="generateScript" name="generateScript" style="float: left; margin-right: 5px" ng-model="isGeneratedScript" />
                                        <label for="generateScript">Generate a PowerShell script to download all the videos.</label>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <div class="alert alert-danger validationError" style="display: none">
                                        The email address is not valid!
                                    </div>
                                    <div class="alert messageBox alert-danger" style="display: none">
                                    </div>
                                    <div class="action-button">
                                        <button type="button" class="btn btn-default" id="closeShareAllVideosModal" data-dismiss="modal" tabindex="1">Cancel</button>
                                        <button type="button" class="btn btn-primary" id="shareAllVideos" tabindex="0" ng-click="shareAllVideos(organizationEmail)" ng-disabled="shareAllVideosForm.organizationEmail.$invalid && shareAllVideosForm.organizationEmail.$dirty || shareAllVideosForm.organizationEmail.$pristine">Send</button>
                                        <button class="btn btn-primary hidden" id="shareVideosLoading">
                                            <img alt="Loading..." src="~/Images/small-load.gif" />
                                        </button>
                                        <button class="btn btn-primary doneSendingEmail hidden" data-dismiss="modal">Done</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    if (User.IsInRole("Global Admin"))
                    {
                        <div id="deleteVideosModal" class="modal fade" tabindex="-1" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h4 class="modal-title">DELETE ALL VIDEOS</h4>
                                    </div>
                                    <div class="modal-body">
                                        <div class="row link-note" ng-hide="listAllVideos === undefined || listAllVideos.length === 0 || listAllVideos === null">
                                            <h5 ng-if="listAllVideos.length == 1">Are you sure you want to delete this video?</h5>
                                            <h5 ng-if="listAllVideos.length > 1">Are you sure you want to delete these {{listAllVideos.length}} videos?</h5>
                                            <br />
                                            <h5>This will unlock all locked videos.</h5>
                                            <h5 ng-if="listAllVideos.length == 1">This video will be permanently deleted in 7 days.</h5>
                                            <h5 ng-if="listAllVideos.length > 1">These {{listAllVideos.length}} videos will be permanently deleted in 7 days.</h5>
                                        </div>
                                        <div class="row" style="overflow:auto;max-height:493px;">
                                            <table id="listDeletingVideos">
                                                <thead>
                                                    <tr class="no-cellspace" style="background-color:#101012;">
                                                        <th>
                                                            <div class="col_title col1">
                                                                NAME
                                                            </div>
                                                        </th>
                                                        <th>
                                                            <div class="sort col_title col2">
                                                                RECORDED
                                                            </div>
                                                        </th>
                                                        <th>
                                                            <div class="duration sort col_title col3">
                                                                LENGTH
                                                            </div>
                                                        </th>
                                                        <th>
                                                            <div class="sort col_title col4">
                                                                UPLOADED
                                                            </div>
                                                        </th>
                                                        <th>
                                                            <div class="sort col_title col4a">
                                                                OFFICER
                                                            </div>
                                                        </th>
                                                        <th>
                                                            <div class="sort col_title col5">
                                                                TYPE
                                                            </div>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr ng-show="isLoadingList" class="no-cellspace ng-hide">
                                                        <td colspan="12"><div style="padding-top:10px;padding-bottom:10px;text-align:center;color:#6f6f6f;">Loading...<br><br><img src="/Images/loading.gif" class="center-block" alt="Loading..."></div></td>
                                                    </tr>
                                                    <tr class="no-cellspace" ng-repeat="videoMetadata in listAllVideos">
                                                        <td><div class="colfield">{{ videoMetadata.Video.FileName }}</div></td>
                                                        <td><div class="colfield">{{ videoMetadata.Video.RecordedDate | date:'MM/dd/yyyy HH:mm:ss' }}</div></td>
                                                        <td><div class="colfield">{{ videoMetadata.Video.Duration | duration:'seconds' }}</div></td>
                                                        <td><div class="colfield">{{ videoMetadata.Video.UploadedDate | date:'MM/dd/yyyy' }}</div></td>
                                                        <td><div class="colfield">{{ videoMetadata.Metadata.OfficerName }}</div></td>
                                                        <td><div class="colfield">{{ videoMetadata.Metadata.EncounterType }}</div></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <div class="alert messageBoxError alert-danger" style="display: none; margin-bottom: 0px; float: left">
                                            Something went wrong, please try again!
                                        </div>
                                        <div class="alert messageBoxSuccess alert-success" style="display: none; margin-bottom: 0px;float: left">
                                            Delete All Videos Successfully!
                                        </div>
                                        <div class="action-button">
                                            <button type="button" class="btn btn-primary" id="deleteToRecycleBin" ng-click="deleteToRecycleBin()" ng-disabled="listAllVideos === undefined || listAllVideos.length === 0 || listAllVideos === null">Delete To Recycle Bin</button>
                                            <button class="btn btn-primary hidden" id="deleteAllVideosLoading">
                                                <img alt="Loading..." src="~/Images/small-load.gif" />
                                            </button>
                                            <button type="button" id="cancelDeleteAllVideosModal" class="btn btn-default" data-dismiss="modal" tabindex="1">Cancel</button>
                                            <button type="button" id="closeDeleteAllVideosModal" style="display: none" class="btn btn-default" data-dismiss="modal" tabindex="1">Close</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    }

                }


            </div>
            <div id="vid_entries_list">
                <div class="entry_container">
                    @*<span class="glyphicon glyphicon-trash recycle-bin" ng-cloak ng-show="isRecycleBin"></span>
                        <span class="glyphicon glyphicon-user my-videos" ng-cloak ng-show="isMyVideo"></span>*@
                    <div id="left-entry-container">
                        <div class="results_label" ng-cloak>
                            SHOWING <br class="br-class" /> <span> {{ totalResults }}<span ng-show="groupByView">/{{totalVideos}}</span> RESULTS</span> <br />PAGE {{pageNumber}} / {{ totalPages }}
                            <span class="glyphicon glyphicon-info-sign" data-toggle="modal" data-target="#infoModal"></span>
                        </div>
                    </div>
                    <div id="center-entry-container">
                        <div class="preset-input-container">
                            @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin"))
                            {
                                <div class="preset_input" ng-hide="$root.isHideDownloadVideos || allVideoMetadata.length == 0">
                                    <div class="wrap-preset-input">
                                        <button class="btn btn-primary custom-button" style="" ng-click="DeleteVideos(videoMetadata,'#shareAllVideosModal')" ng-cloak>Share All Videos</button>
                                    </div>
                                </div>
                                if (User.IsInRole("Global Admin"))
                                {
                                    <div class="preset_input" ng-hide="$root.isHideDownloadVideos || allVideoMetadata.length == 0">
                                        <div class="wrap-preset-input">
                                            <button class="btn btn-primary custom-button" style="" ng-click="DeleteVideos(videoMetadata,'#deleteVideosModal')" ng-cloak>Delete All Videos</button>
                                        </div>
                                    </div>
                                }
                            }
                            <div class="preset_input">
                                <div class="wrap-preset-input">
                                    <button class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important; float: left; margin-right: 2px" type="button" id="btn_multipleVideos" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" ng-click="multipleVideos()">
                                        <span><i class="state-icon glyphicon" ng-class="isEnabledMultiVideos === true ? 'glyphicon-check' : 'glyphicon-unchecked'"></i></span> Select Multiple
                                    </button>
                                    <a href="javascript:void(0)" ng-click="listSelectedVideos.length > 0 ? playMultiVideos(): return;" ng-disabled="listSelectedVideos.length === 0" style="font-size: 22px;float: left;color: white;margin-top: -2px;">
                                        <i class="fa fa-play-circle-o" ng-class="listSelectedVideos.length === 0 ? 'fa-disabled' : ''" aria-hidden="true" ng-cloak></i>
                                    </a>
                                </div>
                            </div>
                            @if (!User.IsInRole("District Attorney") && !User.IsInRole("DA Admin"))
                            {
                                <div class="preset_input">
                                    <div class="wrap-preset-input dropdown page-size pull-left">
                                        <button class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;" type="button" id="dropdownMenu3" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                            <span class="group-by-button-text">Group By</span>
                                            <span class="caret"></span>
                                        </button>
                                        <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
                                            @if (User.IsInRole("Global Admin"))
                                            {
                                                <li ng-repeat="n in ['None', 'Encounter Type', 'Organization'] track by $index">
                                                    <a ng-click="groupBy(n)">{{ n }}</a>
                                                </li>
                                            }
                                            else if (User.IsInRole("Organization Admin")
                                                || User.IsInRole("Officer V2")
                                                || User.IsInRole("View entire department")
                                                || User.IsInRole("Supervisor"))
                                            {
                                                <li ng-repeat="n in ['None', 'Encounter Type', 'Officer'] track by $index">
                                                    <a ng-click="groupBy(n)">{{ n }}</a>
                                                </li>
                                            }
                                            else if (User.IsInRole("Officer")
                                                || User.IsInRole("Corrections"))
                                            {
                                                <li ng-repeat="n in ['None', 'Encounter Type'] track by $index">
                                                    <a ng-click="groupBy(n)">{{ n }}</a>
                                                </li>
                                            }
                                            @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin") || User.IsInRole("Supervisor"))
                                            {
                                                <li>
                                                    <a ng-click="groupBy('Group')">Group</a>
                                                </li>
                                            }
                                        </ul>
                                    </div>
                                </div>
                            }
                            <div class="preset_input recordedby_preset hideDOM">
                                <div class="wrap-preset-input dropdown page-size pull-left">
                                    <button class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;" type="button" id="dropdownMenu4" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" ng-show="groupByView == false">
                                        <span class="recorded-by-button-text">Recorded By</span>
                                        <span class="caret"></span>
                                    </button>
                                    <div class="tooltip-wrapper" title="Pagination is unavailable in this view" ng-cloak ng-show="groupByView == true">
                                        <button class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;" type="button" id="dropdownMenu2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" ng-disabled="groupByView == true">
                                            <span class="recorded-by-button-text">Recorded By</span>
                                            <span class="caret"></span>
                                        </button>
                                    </div>
                                    <ul class="dropdown-menu" aria-labelledby="dropdownMenu4">
                                        <li>
                                            <a ng-click="filterByDevice(-1)">All</a>
                                        </li>
                                        <li>
                                            <a ng-click="filterByDevice(1)">BWC</a>
                                        </li>
                                        <li>
                                            <a ng-click="filterByDevice(2)">Dash Cam</a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="preset_input">
                                <div class="wrap-preset-input dropdown page-size pull-left">
                                    <button class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" ng-show="groupByView == false">
                                        <span class="show-dropdown-text">Show</span>
                                        <span class="caret"></span>
                                    </button>
                                    <div class="tooltip-wrapper" title="Pagination is unavailable in this view" ng-cloak ng-show="groupByView == true">
                                        <button class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" ng-disabled="groupByView == true">
                                            <span class="show-dropdown-text">Show</span>
                                            <span class="caret"></span>
                                        </button>
                                    </div>
                                    <ul class="dropdown-menu" aria-labelledby="dropdownMenu1">
                                        <li ng-repeat="n in [10,25,50,100] track by $index">
                                            <a ng-click="pageSizeChange(n)">{{ n }}</a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="preset_input select-page-dropdown">
                                <div class="wrap-preset-input btn-group">
                                    <button type="button" class="btn btn-default select-page-button" style="padding: 0px 10px !important;" ng-click="getItems(0)" ng-disabled="allVideoMetadata.length == 0" ng-show="groupByView == false">Select Page</button>
                                    <button type="button" class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;margin-left: -3px" data-toggle="dropdown" ng-disabled="allVideoMetadata.length == 0" ng-show="groupByView == false">
                                        <span class="caret"></span>
                                    </button>
                                    <div class="tooltip-wrapper" title="Pagination is unavailable in this view" ng-cloak ng-show="groupByView == true">
                                        <button type="button" class="btn btn-default select-page-button" style="padding: 0px 10px !important;" ng-click="getItems(0)" ng-disabled="groupByView == true">Select Page</button>
                                        <button type="button" class="btn btn-default dropdown-toggle" style="padding: 0px 10px !important;margin-left: -3px" data-toggle="dropdown" ng-disabled="groupByView == true">
                                            <span class="caret"></span>
                                        </button>
                                    </div>
                                    <ul class="dropdown-menu" role="menu">
                                        <li ng-repeat="n in getTotalPages() track by $index">
                                            <a ng-click="getItems($index + 1)">{{$index + 1}}</a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="right-entry-container">
                        <div class="presets">
                            <div id="igainactive" class="view_mode_icon"><a href="#" class="lgallery"><img src="~/Images/icon_gallery_inactive.png"></a></div>
                            <div id="ilactive" class="view_mode_icon"><img src="~/Images/icon_list_active.png"></div>
                        </div>
                        <div class="preset_label">
                            MODE
                        </div>
                    </div>
                    <div class="clear"></div>
                </div>
                <div id="list-view-table-wrapper" class="table-responsive" style="width:100%">
                    <table class="table" style="width:100%;" ng-show="isLoading == false && groupByView == false">
                        <thead>
                            <tr class="no-cellspace" style="background-color:#101012;">
                                <th>
                                    <div class="col_title col1"></div>
                                </th>
                                <th>
                                    <div class="sort col_title col2" ng-click="order('recorded')">
                                        RECORDED<span class="glyphicon"
                                                      ng-class="{ 'glyphicon-triangle-bottom': predicate == 'recorded' && reverse == true || predicate != 'recorded',
                                                                        'glyphicon-triangle-top': predicate == 'recorded' && reverse == false }"></span>
                                    </div>
                                </th>
                                <th>
                                    <div class="duration sort col_title col3" ng-click="order('duration')">
                                        LENGTH<span class="glyphicon" ng-class="{ 'glyphicon-triangle-bottom': predicate == 'duration' && reverse == true || predicate != 'duration',
                                                                                    'glyphicon-triangle-top': predicate == 'duration' && reverse == false }"></span>
                                    </div>
                                </th>
                                <th>
                                    <div class="sort col_title col4" ng-click="order('uploaded')">
                                        UPLOADED<span class="glyphicon" ng-class="{ 'glyphicon-triangle-bottom': predicate == 'uploaded' && reverse == true || predicate != 'uploaded',
                                                                                        'glyphicon-triangle-top': predicate == 'uploaded' && reverse == false }"></span>
                                    </div>
                                </th>
                                <th>
                                    <div class="sort col_title col4a" ng-click="order('officer')">
                                        OFFICER<span class="glyphicon" ng-class="{ 'glyphicon-triangle-bottom': predicate == 'officer' && reverse == true || predicate != 'officer',
                                                                                        'glyphicon-triangle-top': predicate == 'officer' && reverse == false }"></span>
                                    </div>
                                </th>
                                <th>
                                    <div class="sort col_title col5" ng-click="order('type')">
                                        TYPE<span class="glyphicon" ng-class="{ 'glyphicon-triangle-bottom': predicate == 'type' && reverse == true || predicate != 'type',
                                                                                        'glyphicon-triangle-top': predicate == 'type' && reverse == false }"></span>
                                    </div>
                                </th>
                                <th>
                                    <div class="sort col_title col6" ng-click="order('caseNumber')">
                                        CASE #<span class="glyphicon" ng-class="{ 'glyphicon-triangle-bottom': predicate == 'caseNumber' && reverse == true || predicate != 'caseNumber',
                                                                                        'glyphicon-triangle-top': predicate == 'caseNumber' && reverse == false }"></span>
                                    </div>
                                </th>
                                <th>
                                    <div class="sort col_title col8" ng-click="order('tags')">
                                        TAGS<span class="glyphicon" ng-class="{ 'glyphicon-triangle-bottom': predicate == 'tags' && reverse == true || predicate != 'tags',
                                                                                    'glyphicon-triangle-top': predicate == 'tags' && reverse == false }"></span>
                                    </div>
                                </th>
                                <th>
                                    <div class="sort col_title col9" ng-click="order('comments')">
                                        COMMENTS<span class="glyphicon" ng-class="{ 'glyphicon-triangle-bottom': predicate == 'comments' && reverse == true || predicate != 'comments',
                                                                                        'glyphicon-triangle-top': predicate == 'comments' && reverse == false }"></span>
                                    </div>
                                </th>
                                <th>
                                    <div class="sort col_title col11" ng-click="order('isLocked')">
                                        <span class="glyphicon glyphicon-lock"></span>
                                        <span class="glyphicon" ng-class="{ 'glyphicon-triangle-bottom': predicate == 'isLocked' && reverse == true || predicate != 'isLocked',
                                                                                        'glyphicon-triangle-top': predicate == 'isLocked' && reverse == false }"></span>
                                    </div>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr class="no-cellspace" ng-hide="isLoading" id="{{ videoMetadata.VideoMetadataId }}_videoMetadata"
                                ng-repeat="videoMetadata in allVideoMetadata"
                                ng-class="{'selected-row': videoMetadata.VideoMetadataId == selectedId, 'non-selected-row': videoMetadata.VideoMetadataId != selectedId }" ng-click="selectVideoInListMode(videoMetadata)"
                                ngc-done
                                ng-cloak>
                                <td class="celfield">
                                    <div class="text-center col1options">

                                        <a class="pull-left" href="#" title="Play Video" ng-click="setVideo(videoMetadata)" onclick="loadvid()" ng-show="isEnabledMultiVideos === false"><img src="~/Images/icon_play.png"></a>
                                        <span class="pull-left" ng-hide="isHideCheckbox(videoMetadata.VideoMetadataId)" style="" ng-show=""><input type="checkbox" class="styled-checkbox" id="list-view-styled-checkbox-{{videoMetadata.VideoMetadataId}}" ng-checked="listSelectedVideos.indexOf(videoMetadata.VideoMetadataId) !== -1" ng-click="selectVideo(videoMetadata.VideoMetadataId, listSelectedVideos.indexOf(videoMetadata.VideoMetadataId))" /><label for="list-view-styled-checkbox-{{videoMetadata.VideoMetadataId}}"></label></span>
                                        <span class="pull-left" ng-show="isHideCheckbox(videoMetadata.VideoMetadataId) && listSelectedVideos.length === 4" style="width: 30px;height: 20px;background-color: transparent; display: inline-block"></span>
                                        @if (!User.IsInRole("View Only") && !User.IsInRole("District Attorney") && !User.IsInRole("DA Admin"))
                                        {
                                            if (User.IsInRole("Officer V2")
                                                || (User.IsInRole("View entire department") && !User.IsInRole("Supervisor") && !User.IsInRole("Organization Admin") && !User.IsInRole("Global Admin")))
                                            {
                                                <a class="pull-left" href="#" title="Edit Video Details" ng-if="videoMetadata.Video.UserId == '@userId'" ng-click="EditItem(videoMetadata,'#editModal')"><img src="~/Images/icon_edit.png" class="item_ico_edit action"></a>
                                            }
                                            else
                                            {
                                                <a class="pull-left" href="#" title="Edit Video Details" ng-click="EditItem(videoMetadata,'#editModal')"><img src="~/Images/icon_edit.png" class="item_ico_edit action"></a>
                                            }

                                        }

                                        <a class="pull-left" href="#" title="Video Details" style="cursor: pointer" ng-click="ShowDetails(videoMetadata,'#detailModal')"><img src="~/Images/icon_list.png" class="item_ico_list"></a>
                                        @if (User.IsInRole("Global Admin")
|| User.IsInRole("Organization Admin")
|| User.IsInRole("Share"))
                                        {
                                            // SHARE BUTTON
                                            <a class="pull-left" href="javascript:void(0)" title="Share Video" ng-click="ShareVideos(videoMetadata,'#emailModal')" style="font-size: 24px; color: #bebec0">
                                                <i class="fa fa-share-square-o item_ico_share" aria-hidden="true"></i>
                                            </a>
                                        }

                                        @if (User.IsInRole("Global Admin")
|| User.IsInRole("Organization Admin")
|| User.IsInRole("Redact"))
                                        {
                                            <a class="pull-left" href="javascript:void(0)" title="Redact Video" ng-click="ShareVideos(videoMetadata,'#redactModal')">
                                                <img src="~/Images/icon_redact.png" style="cursor: pointer" class="item_ico_redact" />
                                            </a>
                                        }

                                        @if (User.IsInRole("Global Admin")
|| User.IsInRole("Organization Admin")
|| User.IsInRole("Downloader")
|| User.IsInRole("District Attorney")
|| User.IsInRole("DA Admin")
|| User.IsInRole("Download"))
                                        {
                                            // DOWNLOAD BUTTON
                                            <a class="download-movie pull-left" ng-click="downloadVideo(videoMetadata, '#downloadModal')" download>
                                                @*href="/Data/Download/{{videoMetadata.Video.VideoId}}" download="/Data/Download/{{videoMetadata.Video.VideoId}}">*@
                                                <img src="~/Images/icon_download.png" class="item_ico_download" style="cursor: pointer">
                                            </a>
                                        }
                                    </div>
                                </td>
                                <td class="celfield"><div class="colfield col2">{{ GetRecordedDate (videoMetadata) }}</div></td>
                                <td class="celfield"><div class="colfield col3">{{ videoMetadata.Video.Duration | duration:'seconds' }}</div></td>
                                <td class="celfield"><div class="colfield col4">{{ videoMetadata.Video.UploadedDate | date:'MM/dd/yyyy' }}</div></td>
                                <td class="celfield">
                                    <div class="colfield col4a">{{ videoMetadata.Metadata.OfficerName }}</div>
                                </td>
                                <td class="celfield">
                                    <div class="colfield col5">{{ videoMetadata.Metadata.EncounterType }}</div>
                                </td>
                                <td class="celfield">
                                    <div class="colfield col6">{{ videoMetadata.Metadata.CaseNumber }}</div>
                                </td>
                                <td class="celfield">
                                    <div class="colfield col8">{{ videoMetadata.Metadata.Tags }}</div>
                                </td>
                                <td class="celfield">
                                    <div class="colfield col9">{{ videoMetadata.Metadata.Comments | limitTo:300 }} </div>
                                </td>
                                <td class="celfield" align="center" @*style="width:4%"*@>
                                    <div class="colfield col11">
                                        @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin"))
                                        {
                                            <div ng-class="{ 'lock-grid-single': videoMetadata.Video.Deleted == false, 'lock-grid-double': videoMetadata.Video.Deleted == true }">
                                                <span class="glyphicon" ng-class="{'glyphicon-ok': videoMetadata.Video.PreventDeletion, 'glyphicon-remove': !videoMetadata.Video.PreventDeletion }"></span>
                                                <span class="glyphicon glyphicon-trash" ng-show="videoMetadata.Video.Deleted"></span>
                                            </div>
                                        }
                                        else
                                        {
                                            <span class="glyphicon lock-grid-single" ng-class="{'glyphicon-ok': videoMetadata.Video.PreventDeletion, 'glyphicon-remove': !videoMetadata.Video.PreventDeletion }"></span>
                                        }
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table class="table" ng-show="isLoading == false && groupByView == true">
                        <thead>
                            <tr class="no-cellspace" style="background-color:#101012;">
                                <th>
                                    <div class="col_title col1"></div>
                                </th>
                                <th>
                                    <div class="col_title col2">RECORDED</div>
                                </th>
                                <th>
                                    <div class="duration col_title col3">LENGTH</div>
                                </th>
                                <th>
                                    <div class="col_title col4">UPLOADED</div>
                                </th>
                                <th>
                                    <div class="col_title col4a">OFFICER</div>
                                </th>
                                <th>
                                    <div class="col_title col5">TYPE</div>
                                </th>
                                <th>
                                    <div class="col_title col6">CASE #</div>
                                </th>
                                <th>
                                    <div class="col_title col8">TAGS</div>
                                </th>
                                <th>
                                    <div class="col_title col9">COMMENTS</div>
                                </th>
                                <th>
                                    <div class="col_title col11">
                                        <span class="glyphicon glyphicon-lock"></span>
                                    </div>
                                </th>
                            </tr>
                        </thead>
                        <tbody ng-repeat="(key, value) in allVideoMetadata | groupBy: groupByType"
                               ngc-done
                               ng-cloak>
                            <tr>
                                <td colspan="10" style="padding-left:10px;">
                                    <div class="group_section">
                                        <span class="group_title">{{ groupByType === "GroupId" ? getGroupNameById(key, value[0].OrganizationId) : getGroupTitle(key) }}</span>
                                        <span class="group_link">[<a href="#" title="See All" ng-click="seeAllvideos(key)">See All</a>]</span>
                                    </div>
                                </td>
                            </tr>
                            <tr class="no-cellspace" ng-show="isLoading == false && groupByView == true" id="{{ videoMetadata.VideoMetadataId }}_videoMetadata"
                                ng-repeat="videoMetadata in value | limitTo:5"
                                ng-class="{'selected-row': videoMetadata.VideoMetadataId == selectedId, 'non-selected-row': videoMetadata.VideoMetadataId != selectedId }" ng-click="selectVideoInListMode(videoMetadata)"
                                ngc-done
                                ng-cloak>
                                <td class="celfield">
                                    <div class="text-center col1options">
                                        <a class="pull-left" href="#" title="Play Video" ng-click="setVideo(videoMetadata)" onclick="loadvid()" ng-show="isEnabledMultiVideos === false"><img src="~/Images/icon_play.png"></a>
                                        <span class="pull-left" ng-hide="isHideCheckbox(videoMetadata.VideoMetadataId)" style="" ng-show=""><input type="checkbox" class="styled-checkbox" id="list-view-group-by-styled-checkbox-{{videoMetadata.VideoMetadataId}}" ng-checked="listSelectedVideos.indexOf(videoMetadata.VideoMetadataId) !== -1" ng-click="selectVideo(videoMetadata.VideoMetadataId, listSelectedVideos.indexOf(videoMetadata.VideoMetadataId))" /><label for="list-view-group-by-styled-checkbox-{{videoMetadata.VideoMetadataId}}"></label></span>
                                        <span class="pull-left" ng-show="isHideCheckbox(videoMetadata.VideoMetadataId) && listSelectedVideos.length === 4" style="width: 30px;height: 20px;background-color: transparent; display: inline-block"></span>
                                        @if (!User.IsInRole("View Only"))
                                        {
                                            if (User.IsInRole("Officer V2")
                                                || (User.IsInRole("View entire department") && !User.IsInRole("Supervisor") && !User.IsInRole("Organization Admin") && !User.IsInRole("Global Admin")))
                                            {
                                                <a class="pull-left" href="#" title="Edit Video Details" ng-if="videoMetadata.Video.UserId == '@userId'" ng-click="EditItem(videoMetadata,'#editModal')"><img src="~/Images/icon_edit.png" class="item_ico_edit action"></a>
                                            }
                                            else
                                            {
                                                <a class="pull-left" href="#" title="Edit Video Details" ng-click="EditItem(videoMetadata,'#editModal')"><img src="~/Images/icon_edit.png" class="item_ico_edit action"></a>
                                            }
                                        }

                                        <a class="pull-left" href="#" title="Video Details" style="cursor: pointer" ng-click="ShowDetails(videoMetadata,'#detailModal')"><img src="~/Images/icon_list.png" class="item_ico_list"></a>
                                        @if (User.IsInRole("Global Admin")
|| User.IsInRole("Organization Admin")
|| User.IsInRole("Downloader")
|| User.IsInRole("Share"))
                                        {

                                            // SHARE BUTTON
                                            <a class="pull-left" href="javascript:void(0)" title="Share Video" ng-click="ShareVideos(videoMetadata,'#emailModal')" style="font-size: 24px; color: #bebec0">
                                                <i class="fa fa-share-square-o item_ico_share" aria-hidden="true"></i>
                                            </a>
                                        }

                                        @if (User.IsInRole("Global Admin")
|| User.IsInRole("Organization Admin")
|| User.IsInRole("Downloader")
|| User.IsInRole("Redact"))
                                        {
                                            <a class="pull-left" href="javascript:void(0)" title="Redact Video" ng-click="ShareVideos(videoMetadata,'#redactModal')">
                                                <img src="~/Images/icon_redact.png" style="cursor: pointer" class="item_ico_redact" />
                                            </a>
                                        }
                                        @if (User.IsInRole("Global Admin")
|| User.IsInRole("Organization Admin")
|| User.IsInRole("District Attorney")
|| User.IsInRole("DA Admin")
|| User.IsInRole("Downloader")
|| User.IsInRole("Download"))
                                        {
                                            // DOWNLOAD BUTTON
                                            <a class="download-movie pull-left" ng-click="downloadVideo(videoMetadata, '#downloadModal')" download>
                                                @*href="/Data/Download/{{videoMetadata.Video.VideoId}}" download="/Data/Download/{{videoMetadata.Video.VideoId}}">*@
                                                <img src="~/Images/icon_download.png" class="item_ico_download" style="cursor: pointer">
                                            </a>
                                        }
                                    </div>
                                </td>
                                <td class="celfield"><div class="colfield col2" style="width:100%">{{ GetRecordedDate (videoMetadata) }}</div></td>
                                <td class="celfield"><div class="colfield col3" style="width:100%">{{ videoMetadata.Video.Duration | duration:'seconds' }}</div></td>
                                <td class="celfield"><div class="colfield col4" style="width:100%">{{ videoMetadata.Video.UploadedDate | date:'MM/dd/yyyy' }}</div></td>
                                <td class="celfield">
                                    <div class="colfield col4a" style="width:100%">{{ videoMetadata.Metadata.OfficerName }}</div>
                                </td>
                                <td class="celfield">
                                    <div class="colfield col5" style="width:100%">{{ videoMetadata.Metadata.EncounterType }}</div>
                                </td>
                                <td class="celfield">
                                    <div class="colfield col6" style="width:100%">{{ videoMetadata.Metadata.CaseNumber }}</div>
                                </td>
                                <td class="celfield">
                                    <div class="colfield col8" style="width:100%">{{ videoMetadata.Metadata.Tags }}</div>
                                </td>
                                <td class="celfield">
                                    <div class="colfield col9" style="width:100%">{{ videoMetadata.Metadata.Comments | limitTo:300 }} </div>
                                </td>
                                <td class="celfield" align="center">
                                    <div class="colfield col11">
                                        @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin"))
                                        {
                                            <div ng-class="{ 'lock-grid-single': videoMetadata.Video.Deleted == false, 'lock-grid-double': videoMetadata.Video.Deleted == true }">
                                                <span class="glyphicon" ng-class="{'glyphicon-ok': videoMetadata.Video.PreventDeletion, 'glyphicon-remove': !videoMetadata.Video.PreventDeletion }"></span>
                                                <span class="glyphicon glyphicon-trash" ng-show="videoMetadata.Video.Deleted"></span>
                                            </div>
                                        }
                                        else
                                        {
                                            <span class="glyphicon lock-grid-single" ng-class="{'glyphicon-ok': videoMetadata.Video.PreventDeletion, 'glyphicon-remove': !videoMetadata.Video.PreventDeletion }"></span>
                                        }
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div ng-show="allVideoMetadata.length == 0 && !isLoading">
                        <div class="text-center">No Videos Found</div>
                    </div>
                    <div ng-show="isLoading">
                        <div style="padding-top:100px;padding-bottom:100px;text-align:center;color:#6f6f6f;">Loading...<br><br><img src="/Images/loading.gif" class="center-block" alt="Loading..." /></div>
                    </div>
                </div>
                <div class="paginationl4" ng-cloak ng-show="!isLoading">
                    <a href="javascript:void(0)" onmousedown="return false" ng-class="{ disabled: pageNumber == 1 }" aria-label="Previous" ng-click="first()" ng-disabled="pageNumber == 1 || groupByView == true"><span aria-hidden="true"><< FIRST</span></a> &nbsp;&nbsp; <a href="javascript:void(0)" onmousedown="return false" aria-label="Previous" ng-click="previous()" ng-class="{ disabled: pageNumber == 1 }" ng-disabled="pageNumber == 1 || groupByView == true"><span aria-hidden="true">< PREV</span></a> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <a href="javascript:void(0)" onmousedown="return false" aria-label="Next" ng-click="next()" ng-class="{ disabled: pageNumber == (totalPages) || totalPages == 0 }" ng-disabled="pageNumber == (totalPages) || groupByView == true || totalPages == 0"><span aria-hidden="true">NEXT ></span></a> &nbsp;&nbsp; <a href="javascript:void(0)" onmousedown="return false" aria-label="Last" ng-click="last()" ng-class="{ disabled: pageNumber == (totalPages) || totalPages == 0}" ng-disabled="pageNumber == (totalPages) || groupByView == true || totalPages == 0"><span aria-hidden="true">LAST >></span></a>
                </div>
            </div>
            <div id="emailModal" class="modal fade" tabindex="-1" role="dialog" ng-form="emailModalForm">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">SHARE VIDEO</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row link-note">
                                <h5>This link will expire in 24 hours</h5>
                            </div>
                            <div class="row">
                                <input type="email" id="receiverEmail" class="form-control" name="receiverEmail" placeholder="Please enter receiver's email" ng-model="receiverEmail" required />
                                <br />
                                <div style="margin:auto;width: 96%">
                                    <div class="col-md-6 text-center">
                                        <input id="partialMode" name="partialMode" type="checkbox" value="" style="vertical-align: middle;margin-bottom: 6px;" ng-model="enablePartialClipsMode" />
                                        <label for="partialMode">Control how much of the video is sent</label>
                                    </div>
                                    <div class="col-md-6 text-center" style="border-left: 1px solid #ccc;">
                                        <input id="viewOnlyMode" name="viewOnlyMode" type="checkbox" value="" style="vertical-align: middle;margin-bottom: 6px;" ng-model="enableViewOnlyMode" ng-disabled="enablePartialClipsMode" />
                                        <label for="viewOnlyMode">View Only</label>
                                    </div>
                                </div>
                            </div>
                            <div ng-show="enablePartialClipsMode">
                                <input type="hidden" id="itemDuration" value="{{ selectedItem.Video.Duration | duration:'seconds' }}" />
                                <hr style="margin-top: 10px; margin-bottom: 10px;" />
                                <div class="row">
                                    <div class="col-xs-6" style="text-align:center">Start playing at</div>
                                    <div class="col-xs-6" style="text-align:center">Stop playing at</div>
                                </div>
                                <div class="row" style="width: 100%; margin: auto;margin-top: 10px;">
                                    <div class="col-xs-6 startingPoints" style="text-align:center; font-size: 10px;">
                                        H: <input id="startingHours" class="partialTime" name="value" value=0 size=3 />
                                        M: <input id="startingMinutes" class="partialTime" name="value" value=0 size=2 /> S: <input id="startingSeconds" class="partialTime" name="value" value=0 size=2 />
                                    </div>
                                    <div class="col-xs-6 endingPoints" style="text-align:center; border-left: 1px solid #ccc;font-size: 10px;">
                                        H: <input id="endingHours" class="partialTime" name="value" value=0 size=3 />
                                        M: <input id="endingMinutes" class="partialTime" name="value" value=0 size=2 /> S: <input id="endingSeconds" class="partialTime" name="value" value=0 size=2 />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="alert alert-danger validationError" style="display: none">
                                The email address is not valid!
                            </div>
                            <div class="alert messageBox alert-danger" style="display: none" ng-click="copyUrl()">
                            </div>
                            <div class="action-button">
                                <button type="button" class="btn btn-default" id="closeEmailModal" data-dismiss="modal" tabindex="1">Cancel</button>
                                <button type="button" class="btn btn-primary" id="emailVideo" tabindex="0" ng-click="sendEmail(selectedItem, receiverEmail)">Send</button>
                                <button class="btn btn-primary hidden" id="emailLoading">
                                    <img alt="Loading..." src="~/Images/small-load.gif" />
                                </button>
                                <button class="btn btn-primary doneSendingEmail hidden" data-dismiss="modal">Done</button>
                            </div>
                            <input type="text" value="" id="linktoCopy" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="redactModal" class="modal fade" tabindex="-1" role="dialog" ng-form="redactModalForm">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">REDACT VIDEO</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row link-note">
                                <h5>The redaction process will take a while to finish. Please specify an email address to be notified when it's done.</h5>
                            </div>
                            <div class="row" style="width: 99%; margin:auto;">
                                <input type="email" name="redactionEmail" id="redactionEmail" value="" class="form-control redactionEmail" placeholder="Email address (leave empty to use your account email address)" ng-model="redactionEmail" ng-disabled="isDoneCheckingBlurredVideos === false" />
                                @*<br />
                                    <input id="customemail" name="customemail" type="checkbox" value="" style="vertical-align: middle;margin-bottom: 6px;" />
                                    <label for="customemail">Use another email</label>*@
                            </div>
                            <div class="row" style="width: 99%; margin: auto; margin-top: 8px;">
                                <div>
                                    <input type="checkbox" id="selectFaces" name="selectFaces" style="float: left; margin-right: 5px;" ng-model="isEnabledSelectFaces" ng-disabled="hasBlurredAll">
                                    <label for="selectFaces">Select faces to blur</label>
                                </div>
                                <div>
                                    <input type="checkbox" id="redactVideoAndAudio" name="redactVideoAndAudio" style="float: left; margin-right: 5px;" ng-model="isEnabledRedactVideoAndAudio">
                                    <label for="redactVideoAndAudio">Redact Video and Audio</label>
                                </div>
                            </div>
                            <div class="row" style="width: 99%; margin: auto; margin-top: 8px;" ng-show="isEnabledRedactVideoAndAudio">
                                <div class="col-xs-6">
                                    <input type="checkbox" id="redactAudioOnly" name="redactAudioOnly" style="float: left; margin-right: 5px;" ng-disabled="!isEnabledRedactVideoAndAudio" ng-model="redactAudioOnly">
                                    <label for="redactAudioOnly">Redact Audio only</label>
                                </div>
                                <div class="col-xs-6">
                                    <a href="#" class="btn btn-default link-with-default-color pull-right" style="padding: 0 5px; color: #000000 !important; font-size: 12px;" ng-click="addNewAudioPart()">Add new part</a>
                                </div>
                            </div>
                            <div class="audioParts" ng-show="isEnabledRedactVideoAndAudio">
                                <input type="hidden" class="itemAudioDuration" value="{{ selectedItem.Video.Duration | duration:'seconds' }}" />
                                <hr style="margin-top: 10px; margin-bottom: 10px;" />
                                <div class="row" style="width: 100%; margin: auto; margin-top: 10px;">
                                    <div class="col-xs-2">
                                    </div>
                                    <div class="col-xs-5" style="text-align: center">Start muting at</div>
                                    <div class="col-xs-5" style="text-align: center">Stop muting at</div>
                                </div>
                                <div class="row audioPart" style="width: 100%; margin: auto; margin-top: 10px;">
                                    <div class="col-xs-2">
                                        <a href="#" class="btn btn-default includeAudioPart link-with-default-color" style="padding: 0 5px;color:  #000000!important;font-size:  12px;" disabled is-included=false>Include this part</a>
                                    </div>
                                    <div class="col-xs-5 startingAudioPoints" style="text-align: center; font-size: 10px;">
                                        H: <input class="audioPartialTime startingAudioHours" name="value" value=0 size=3 />
                                        M: <input class="audioPartialTime startingAudioMinutes" name="value" value=0 size=2 /> S: <input class="audioPartialTime startingAudioSeconds" name="value" value=0 size=2 />
                                    </div>
                                    <div class="col-xs-5 endingAudioPoints" style="text-align: center; border-left: 1px solid #ccc; font-size: 10px;">
                                        H: <input class="audioPartialTime endingAudioHours" name="value" value=0 size=3 />
                                        M: <input class="audioPartialTime endingAudioMinutes" name="value" value=0 size=2 /> S: <input class="audioPartialTime endingAudioSeconds" name="value" value=0 size=2 />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="alert alert-danger validationError" style="display: none">
                                The email address is not valid!
                            </div>
                            <div class="alert messageBox alert-danger" style="display: none">
                            </div>
                            <div class="action-button">
                                <button type="button" class="btn btn-default" id="closeRedactionEmailModal" data-dismiss="modal" tabindex="1">Cancel</button>
                                <button type="button" class="btn btn-primary emailAudioRedactionVideo" id="emailRedactionVideo" tabindex="0" ng-click="redactVideo(redactionEmail,selectedItem.VideoId)" ng-disabled="isDoneCheckingBlurredVideos === false">Proceed</button>
                                <button class="btn btn-primary hidden" id="redactemailLoading">
                                    <img alt="Loading..." src="~/Images/small-load.gif" />
                                </button>
                                <button class="btn btn-primary doneSendingRedactionEmail hidden mgt-10" data-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="editModal" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">EDIT VIDEO</h4>
                        </div>
                        <div class="modal-body">
                            <div class="container" style="width: auto">
                                <div class="row">
                                    <div class="col-md-2 bold">Recorded Date</div><div class="col-md-4">{{ GetRecordedDate(selectedItem) }}</div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 bold">Duration</div><div class="col-md-4">{{ selectedItem.Video.Duration | duration:'seconds' }}</div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 bold">Uploaded Date</div><div class="col-md-4">{{ selectedItem.Video.UploadedDate | date:'MM/dd/yyyy' }}</div>
                                </div>

                                <div class="row" ng-show="selectedItem.Video.DeviceType != 2">
                                    <div class="col-md-2 bold">Officer</div><div class="col-md-4">{{ selectedItem.Metadata.OfficerName }}</div>
                                </div>
                                <div class="row" ng-show="selectedItem.Video.DeviceType != 2">
                                    <div class="col-md-2 bold">Camera SN</div><div class="col-md-4">
                                        {{ selectedItem.Video.CameraSerialNumber }}
                                    </div>
                                </div>

                                <div class="row" ng-show="selectedItem.Video.DeviceType == 2">
                                    <div class="col-md-2 bold">Officers</div>
                                    <div class="col-md-4">{{ selectedVideoVehicleOfficers }}</div>
                                </div>
                                <div class="row" ng-show="selectedItem.Video.DeviceType == 2">
                                    <div class="col-md-2 bold">Vehicle</div>
                                    <div class="col-md-4">{{ selectedVideoVehicleType }}</div>
                                </div>

                                <div class="row">
                                    <div class="col-md-2 bold">Location</div><div class="col-md-4">{{ selectedItem.Video.Location }}</div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 col-md-modal bold">Type</div><div class="col-md-4 col-md-modal" style="padding-top:0px !important;">
                                        <select ng-model="selectedItem.Metadata.EncounterType" class="form-control" style="width: 100px;">
                                            <option ng-repeat="option in types" value="{{option}}">{{option}}</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 col-md-modal bold">Case Number</div>
                                    <div class="col-md-4 col-md-modal" style="padding-top:0px !important;">
                                        <input type="text" ng-model="selectedItem.Metadata.CaseNumber" class="form-control" value="{{ selectedItem.Metadata.CaseNumber }}" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 bold">Title</div>
                                    <div class="col-md-4" style="white-space:nowrap;">
                                        {{ selectedItem.Metadata.Name }}
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 col-md-modal bold">Tags</div>
                                    <div class="col-md-4 col-md-modal" style="padding-top:0px !important;">
                                        <input type="text" ng-model="selectedItem.Metadata.Tags" class="form-control" value="{{ selectedItem.Metadata.Tags }}" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 col-md-modal bold">Comments</div>
                                    <div class="col-md-4 col-md-modal" style="padding-top:0px !important;">
                                        <input type="text" ng-model="selectedItem.Metadata.Comments" class="form-control" value="{{ selectedItem.Metadata.Comments }}" />
                                    </div>
                                </div>
                                <!-- <div class="row">
                                    <div class="col-md-2 col-md-modal bold">Status</div>
                                    <div class="col-md-4 col-md-modal" style="padding-top:0px !important;">
                                        {{ selectedItem.Metadata.Status }}
                                    </div>
                                </div> -->
                                <div class="row">
                                    <div class="col-md-2 bold col-md-modal">Locked</div>

                                    <div class="col-md-4 col-md-modal">

                                        @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin"))
                                        {
                                            <input type="checkbox" ng-model="selectedItem.Video.PreventDeletion" />
                                        }
                                        else
                                        {
                                            <span class="glyphicon" ng-class="{'glyphicon-ok': selectedItem.Video.PreventDeletion, 'glyphicon-remove': !selectedItem.Video.PreventDeletion }"></span>
                                        }
                                    </div>
                                </div>

                                @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin"))
                                {
                                    <div class="row">
                                        <div class="col-md-2 bold" style="height:46px;">Delete</div>
                                        <div class="col-md-4">
                                            <button type="button" class="btn" ng-click="markDeletedStatus(selectedItem)" ng-class="{ 'btn-primary': selectedItem.Video.Deleted == false, 'btn-danger': selectedItem.Video.Deleted == true}" ng-disabled="selectedItem.Video.PreventDeletion">{{ selectedItem.Video.Deleted? 'Un-Delete' : 'Delete' }}</button>
                                        </div>
                                    </div>
                                    <div class="row" ng-show="selectedItem.Video.DeletedDate">
                                        <div class="col-md-2 bold">Delete Date</div>
                                        <div class="col-md-4">
                                            {{ selectedItem.Video.DeletedDate | date:'MM/dd/yyyy' }}
                                        </div>
                                    </div>
                                }


                                @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin"))
                                {
                                    <div class="row">
                                        <div class="col-md-2 bold" style="height:46px;">Reports</div>
                                        <div class="col-md-4">
                                            <ul class="list-group">
                                                <li class="list-group-item">
                                                    <a href="#" class="reportItemLink">Chain of Custody</a>
                                                    <input id="item_Class" name="item.Class" type="hidden" value="Plexus.L4.Reporting.Reports.ChainOfCustody, Plexus.L4.Reporting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null">
                                                    <input id="params" type="text" value='[{"Key":"VideoId","Value":"{{ selectedItem.Video.VideoId}}"}]' style="display:none;" />
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                }
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <button type="button" class="btn btn-primary" ng-click="saveItem(selectedItem)">Save changes</button>
                        </div>
                    </div><!-- /.modal-content -->
                </div><!-- /.modal-dialog -->
            </div><!-- /.modal -->
            <div id="detailModal" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                            <h4 class="modal-title">VIDEO DETAILS</h4>
                        </div>
                        <div class="modal-body">
                            <div class="container" style="width: auto">
                                <div class="row">
                                    <div class="col-md-2 bold">Recorded Date</div>
                                    <div class="col-md-4">
                                        {{ GetRecordedDate(selectedItem) }}
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 bold">Duration</div>
                                    <div class="col-md-4">{{ selectedItem.Video.Duration | duration:'seconds' }}</div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 bold">Uploaded Date</div>
                                    <div class="col-md-4">{{ selectedItem.Video.UploadedDate | date:'MM/dd/yyyy' }}</div>
                                </div>

                                <div class="row" ng-show="selectedItem.Video.DeviceType != 2">
                                    <div class="col-md-2 bold">Officer</div>
                                    <div class="col-md-4">{{ selectedItem.Metadata.OfficerName }}</div>
                                </div>
                                <div class="row" ng-show="selectedItem.Video.DeviceType != 2">
                                    <div class="col-md-2 bold">Camera SN</div>
                                    <div class="col-md-4">{{ selectedItem.Video.CameraSerialNumber }}</div>
                                </div>

                                <div class="row" ng-show="selectedItem.Video.DeviceType == 2">
                                    <div class="col-md-2 bold">Officers</div>
                                    <div class="col-md-4">{{ selectedVideoVehicleOfficers }}</div>
                                </div>
                                <div class="row" ng-show="selectedItem.Video.DeviceType == 2">
                                    <div class="col-md-2 bold">Vehicle</div>
                                    <div class="col-md-4">{{ selectedVideoVehicleType }}</div>
                                </div>

                                <div class="row">
                                    <div class="col-md-2 bold">Type</div>
                                    <div class="col-md-4">
                                        <div>{{ selectedItem.Metadata.EncounterType }}</div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 bold">Case Number</div>
                                    <div class="col-md-4">
                                        <div>{{ selectedItem.Metadata.CaseNumber }}</div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 bold">Title</div>
                                    <div class="col-md-4">
                                        <div style="white-space: nowrap;">{{ selectedItem.Metadata.Name }}</div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 bold">Tags</div>
                                    <div class="col-md-4">
                                        <div>{{ selectedItem.Metadata.Tags }}</div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-2 bold">Comments</div>
                                    <div class="col-md-4">
                                        <div>{{ selectedItem.Metadata.Comments }}</div>
                                    </div>
                                </div>
                                <!-- <div class="row">
                                    <div class="col-md-2 bold">Status</div>
                                    <div class="col-md-4">
                                        {{ selectedItem.Metadata.Status }}
                                    </div>
                                </div> -->
                                <div class="row">
                                    <div class="col-md-2 bold">Locked</div>
                                    <div class="col-md-4">
                                        <span class="glyphicon" ng-class="{'glyphicon-ok': selectedItem.Video.PreventDeletion}"></span>
                                    </div>
                                </div>


                                @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin"))
                                {
                                    <div class="row" ng-show="selectedItem.Video.Deleted">
                                        <div class="col-md-2 bold" style="height: 32px">Deleted</div>
                                        <div class="col-md-4">
                                            <span class="glyphicon" ng-class="{'glyphicon-trash': selectedItem.Video.Deleted}" style="width:100%; padding-left: 26px; height: 26px; line-height: 26px; display:block; background: white;"></span>
                                        </div>
                                    </div>
                                    <div class="row" ng-show="selectedItem.Video.DeletedDate">
                                        <div class="col-md-2 bold">Deleted Date</div>
                                        <div class="col-md-4">
                                            {{ selectedItem.Video.DeletedDate | date:'MM/dd/yyyy' }}
                                        </div>
                                    </div>
                                }

                                @if (User.IsInRole("Global Admin") || User.IsInRole("Organization Admin"))
                                {
                                    <div class="row">
                                        <div class="col-md-2 bold" style="height: 36px;">Reports</div>
                                        <div class="col-md-4" style="width: 194px">
                                            <ul class="list-group" style="margin-bottom:0px">
                                                <li class="list-group-item">
                                                    <a href="#" class="reportItemLink">Chain of Custody</a>
                                                    <input id="item_Class" name="item.Class" type="hidden" value="Plexus.L4.Reporting.Reports.ChainOfCustody, Plexus.L4.Reporting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null">
                                                    <input id="params" type="text" value='[{"Key":"VideoId","Value":"{{ selectedItem.Video.VideoId}}"}]' style="display:none;" />
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                }
                                <div class="row" ng-show="filterRedactedVideos(selectedItem.RedactedVideos).length > 0 || selectedItem.RedactedAudios.length > 0">
                                    <div class="col-md-2 bold" style="height: 36px;">Redacted Versions</div>
                                    <div class="col-md-4" style="width: 194px">
                                        <div class="panel-group" id="accordion">
                                            <div class="panel panel-default" style="border: none;" ng-repeat="(key, value) in filterRedactedVideos(selectedItem.RedactedVideos) | groupBy : 'RedactionType'">
                                                <div class="panel-heading" style="background-color: #000;color: #2f99ff;">
                                                    <h4 class="panel-title" style="font-size: 13px !important;">
                                                        <a data-toggle="collapse" data-parent="#accordion" href="javascript:void(0)" data-target="#collapse{{key}}">
                                                            {{value[0].RedactionType === 1 ? 'All faces blurred' : 'Selected faces blurred'}}
                                                        </a>
                                                    </h4>
                                                </div>
                                                <div id="collapse{{key}}" class="panel-collapse collapse">
                                                    <div class="panel-body" style="padding: 1px;">
                                                        <ul class="list-group" style="margin-bottom:0px">
                                                            <li class="list-group-item" ng-repeat="v in value">
                                                                <a href="javascript:void(0)" class="redactedLink" ng-click="getRedactedLink(true, v.AssetId, selectedItem.Video.AudioOriginated, '#downloadModal')">Download</a>
                                                                <a>|</a>
                                                                <a href="javascript:void(0)" class="redactedLink" ng-click="getRedactedLink(false, v.AssetId, selectedItem.Video.AudioOriginated, '#downloadModal')">Play</a>
                                                                <a>|</a>
                                                                <a href="javascript:void(0)" class="redactedLink" ng-click="deleteRedactedVideos($event, v.AssetId, selectedItem.Video.AudioOriginated)">{{ v.Deleted === true ? 'Undelete' : 'Delete'}}</a>
                                                                <a style="font-size: 12px; cursor: default; text-decoration: none" ng-show="v.Deleted === true">{{v.DeletedDate | date:'MM/dd/yyyy'}}</a>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="panel panel-default" style="border: none;" ng-show="selectedItem.RedactedAudios.length > 0">
                                                <div class="panel-heading" style="background-color: #000;color: #2f99ff;">
                                                    <h4 class="panel-title" style="font-size: 13px !important;">
                                                        <a data-toggle="collapse" data-parent="#accordion" href="javascript:void(0)" data-target="#collapse_audio{{key}}">
                                                            Only audio redacted
                                                        </a>
                                                    </h4>
                                                </div>
                                                <div id="collapse_audio{{key}}" class="panel-collapse collapse">
                                                    <div class="panel-body" style="padding: 1px;">
                                                        <ul class="list-group" style="margin-bottom:0px">
                                                            <li class="list-group-item" ng-repeat="v in selectedItem.RedactedAudios">
                                                                <a href="javascript:void(0)" class="redactedLink" ng-click="getRedactedLink(true, v.AssetId, true, '#downloadModal')">Download</a>
                                                                <a>|</a>
                                                                <a href="javascript:void(0)" class="redactedLink" ng-click="getRedactedLink(false, v.AssetId, true, '#downloadModal')">Play</a>
                                                                <a>|</a>
                                                                <a href="javascript:void(0)" class="redactedLink" ng-click="deleteRedactedVideos($event, v.AssetId, true)">{{ v.Deleted === true ? 'Undelete' : 'Delete'}}</a>
                                                                <a style="font-size: 12px; cursor: default; text-decoration: none" ng-show="v.Deleted === true">{{v.DeletedDate | date:'MM/dd/yyyy'}}</a>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                        </div>
                    </div><!-- /.modal-content -->
                </div><!-- /.modal-dialog -->
            </div><!-- /.modal -->
            <div id="downloadModal" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 id="downloadModal_title" class="modal-title">DOWNLOAD</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row link-note" style="margin-top: 30px; margin-bottom: 20px;">
                                <div class="hidden" id="downloadModal_loadingIcon">
                                    <div class="text-center">Generating video URL...</div><br />
                                    <img src="~/Images/small-load.gif" class="center-block" alt="Loading..." />
                                </div>
                                <div class="hidden" id="downloadModal_errorMessage">
                                    <div class="text-center"><h5>Failed to generate video URL.</h5></div>
                                </div>
                                <div id="downloadModal_bodyMessage">
                                    <h5>To download the video, right click <a id="downloadModal_downloadLink" href="#">here</a> then choose "Save link as..." (Firefox, Chrome) or "Save target as..." (IE, Edge).</h5>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="action-button">
                                <button type="button" class="btn btn-default" data-dismiss="modal" tabindex="1">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="errorModal" class="modal fade modal-error" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title" id="myModalLabel">Validation Failed</h4>
                        </div>
                        <div class="modal-body">
                            <span ng-repeat="e in statusMessage">{{e}} <br /></span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="custom-overlay" ng-show="isShowMultiPlayer" ng-cloak></div>
            <div id="multiplayer" ng-show="isShowMultiPlayer" ng-cloak>
                <div style="height: 5%">
                    <span class="close" style="float: right;font-size: 47px;margin-right: 10px; opacity: 0.5;" ng-click="closeMultiPlayer()">×</span>
                </div>
                <div ng-show="isLoadingMultiPlayer === true" class="imageLoading">
                    <i class="fa fa-circle-o-notch fa-spin fa-3x fa-fw"></i>
                    <span class="sr-only">Loading...</span>
                </div>
                <div class="videoWrapper" ng-show="!isLoadingMultiPlayer">
                    <div style="height: 98%" class="wrapVideo" id="wrapVideo">
                        <div class="sub-video">
                            <video id="multi_player_1" class="video azuremediaplayer amp-big-play-centered amp-default-skin" poster="~/Content/no_record_selected.png" mediagroup="videoMG1">
                                <p class="amp-no-js">
                                    To view this video please enable JavaScript, and consider upgrading to a web browser that supports HTML5 video
                                </p>
                            </video>
                        </div>
                        <div class="sub-video">
                            <video id="multi_player_2" class="video azuremediaplayer amp-big-play-centered amp-default-skin" poster="~/Content/no_record_selected.png" mediagroup="videoMG1">
                                <p class="amp-no-js">
                                    To view this video please enable JavaScript, and consider upgrading to a web browser that supports HTML5 video
                                </p>
                            </video>
                        </div>
                        <br />
                        <div class="sub-video">
                            <video id="multi_player_3" class="video azuremediaplayer amp-big-play-centered amp-default-skin" poster="~/Content/no_record_selected.png" mediagroup="videoMG1">
                                <p class="amp-no-js">
                                    To view this video please enable JavaScript, and consider upgrading to a web browser that supports HTML5 video
                                </p>
                            </video>
                        </div>
                        <div class="sub-video">
                            <video id="multi_player_4" class="video azuremediaplayer amp-big-play-centered amp-default-skin" poster="~/Content/no_record_selected.png" mediagroup="videoMG1">
                                <p class="amp-no-js">
                                    To view this video please enable JavaScript, and consider upgrading to a web browser that supports HTML5 video
                                </p>
                            </video>
                        </div>
                        <div class="BookmarkTooltipsInMultiPlayer"></div>
                        <div class="modal fade" id="deleteBoomarkModal" tabindex="-1" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h4 class="modal-title">Confirm Deletion</h4>
                                    </div>
                                    <div class="modal-body">
                                        Are you sure you want to delete this bookmark?
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                        <button type="button" class="btn btn-primary" id="btnDeleteConfirmInMultiPlayer">Delete Bookmark</button>
                                    </div>
                                </div><!-- /.modal-content -->
                            </div><!-- /.modal-dialog -->
                        </div><!-- /.modal -->
                        <div class="modal fade modal-error in" id="titleIsNotValidInMultiPlayer" tabindex="-1" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h4 class="modal-title">Error</h4>
                                    </div>
                                    <div class="modal-body">
                                        This title is invalid. Please choose another one!
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                    </div>
                                </div><!-- /.modal-content -->
                            </div><!-- /.modal-dialog -->
                        </div><!-- /.modal -->
                    </div>
                </div>
                <div style="height: 5%; width: 100%" class="controlAll" ng-show="!isLoadingMultiPlayer">
                    <div style="width: 500px; margin: auto;background-color: #3c454f; padding: 4px">
                        <div class="btnPlay btn" title="Play/Pause video"></div>
                        <div id="slidecontainer" style="padding-top: 11px" class="btn">
                            <div type="range" min="1" max="5" value="3" class="rangeSlider" id="myRange"></div>
                        </div>
                        <div class="btn manualFrame" style="width: 200px; height: 35px;">
                            <div class="revert5frame" style="width: 25%; height: 100%; float: left">-5</div>
                            <div class="revert1frame" style="width: 25%;height: 100%; float: left">-1</div>
                            <div class="advance1frame" style="width: 25%;height: 100%; float: left">+1</div>
                            <div class="advance5frame" style="width: 25%;height: 100%; float: left">+5</div>
                        </div>
                        <div class="btnMute btn" title="Volume"><i class="fa fa-volume-up " aria-hidden="true"></i></div>
                    </div>
                </div>
                <div style="height: 5%"></div>
            </div>
        </div>
        <div id="infoModal" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Information</h4>
                    </div>
                    <div class="modal-body">
                        <div><span class="bold">Results</span> - Total Results from the current query/resultset</div>
                        <div><span class="bold">Page</span> - Current page of data(10 items per page). </div>

                        <div style="margin-top: 20px;">
                            When sorting, the page will remain the same.
                            For example, if on<span class="bold">Page</span> 2, when sorting by 'Recorded', the data displayed will be Page 2 of the sorted results.

                        </div>
                        <div style="margin-top: 20px">
                            When editing inline textboxes, hit <span class="bold">Enter/Return</span> on the keyboard to commit the changes.

                        </div>
                        <div style="margin-top: 20px">
                            To download a movie file - click the download button <div class="glyphicon glyphicon-download-alt"></div>. Note that how the file is downloaded is dependant on the browser.
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div><!-- /.modal-content -->
            </div><!-- /.modal-dialog -->
        </div><!-- /.modal -->
        <div id="videocontainerLB"></div>
        <div class="viewer row" id="videocontainer" ng-controller="VideoController">
            <div>
                <div style="margin: 0 auto; width: 720px;">
                    <input type="hidden" id="hack" />
                    <div class="video-watermark-container" videobuttons id="watermarkContainer">
                        @*<video id="vjsPlayer" class="video-js vjs-default-skin vjs-big-play-centered video-border"*@
                        <video id="vjsPlayer" class="azuremediaplayer amp-default-skin amp-big-play-centered video-border"
                               width="720" height="405">
                            @*<source src="" type="video/mp4" />*@@*This line causes player error when user first logs on or returns to this page *@
                            <p class="vjs-no-js">
                                To view this video please enable JavaScript, and consider upgrading to a web browser that <a href="http://videojs.com/html5-video-support/" target="_blank">supports HTML5 video</a>
                            </p>
                        </video>
                        <div id="watermark"></div>
                        <div id="loadingStreamUrl">
                            <img alt="Loading..." src="~/Images/small-load.gif" /> Loading Stream Url
                        </div>
                        <div id="reloadPanel">
                            <a href="javascript:void(0)" id="reloadVideo" ng-click="reloadVideo()"><i class="fa fa-refresh" aria-hidden="true"></i></a>
                        </div>
                        <div class="BookmarkTooltips"></div>
                        <div class="modal fade" id="confirm" tabindex="-1" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h4 class="modal-title">Confirm Deletion</h4>
                                    </div>
                                    <div class="modal-body">
                                        Are you sure you want to delete this bookmark?
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                        <button type="button" class="btn btn-primary" id="btnDeleteConfirm">Delete Bookmark</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal fade modal-error in" id="titleIsNotValid" tabindex="-1" role="dialog">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h4 class="modal-title">Error</h4>
                                    </div>
                                    <div class="modal-body">
                                        This title is invalid. Please choose another one!
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div style="position:absolute;top:-20px;right:-20px;" id="closebtn" ng-click="closeVideo()"><img src="~/Content/closebtn.png" /></div>
            </div>
        </div>
    </div>
}
else
{
    //nothing
}
@section scripts
                                        {
    @*@Scripts.Render("//cdnjs.cloudflare.com/ajax/libs/video.js/5.5.3/video.min.js")*@
    @*@Scripts.Render("//cdnjs.cloudflare.com/ajax/libs/video.js/5.7.1/video.min.js")*@
    @*
        Azure Media Player (AMP) doesn't play locally; only available via CDN.
    *@
    @*@Scripts.Render("//amp.azure.net/libs/amp/1.6.2/azuremediaplayer.min.js")*@
    @*@Scripts.Render("~/Scripts/azuremediaplayer.min.js")*@
    @*@Scripts.Render("//cdnjs.cloudflare.com/ajax/libs/qtip2/2.2.1/jquery.qtip.min.js")*@
    @Scripts.Render("//cdnjs.cloudflare.com/ajax/libs/qtip2/2.2.1/imagesloaded.pkg.min.js")

    @if (!User.IsInRole("Global Admin") && !User.IsInRole("Organization Admin"))
    {
        <script>
            (function ($) {
                $(function () {
                    if ('@userId') {
                        var searchScope = angular.element($('div[ng-controller=MetaDataSearchController]')[0]).scope();
                        searchScope.userId = '@userId';
                    }
                });
            })(jQuery);

        </script>
    }

    @if (Request.IsAuthenticated)
    {
        <script>
            (function ($) {
                $(function () {
                    var name = '@ViewBag.FullName';
                    $('#watermark').text(name);
                });
            })(jQuery);
        </script>
    }
}

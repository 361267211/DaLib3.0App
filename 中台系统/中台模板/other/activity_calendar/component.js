function other_activity_calendar() {
    var other_activity_calendar_html = `<div class="temp-activity-warp">
<div class="main-title-temp"><span>活动日历</span><span class="e-title"></span><span class="r-btn" @click="other_activity_calenddar_openurl(other_activity_calenddar_more_url)">查看更多 ></span></div>
<div class="activity-content c-l">
    <div class="date-warp">
        <div class="date-title">{{other_activity_calendar_currentYear}}年{{other_activity_calendar_currentMonth}}月
            <span class="date-left" @click="pickPre(other_activity_calendar_currentYear,other_activity_calendar_currentMonth)"></span>
            <span class="date-right" @click="pickNext(other_activity_calendar_currentYear,other_activity_calendar_currentMonth)"></span>
        </div>
        <div class="date-list">
            <div class="title c-l">
                <span class="col">一</span>
                <span class="col">二</span>
                <span class="col">三</span>
                <span class="col">四</span>
                <span class="col">五</span>
                <span class="col">六</span>
                <span class="col">日</span>
            </div>
            <div class="data c-l">
                <span class="col child_color_bg_span_hover" :data-day="new Date(dayobject.day).toLocaleString().slice(0,10)" v-for="(dayobject,i) in other_activity_calendar_days" :class="dayobject.day.getMonth()+1 != other_activity_calendar_currentMonth?'other-month':''" @click.stop="other_dayClick(dayobject)">
                    <span  @click.stop="other_dayClick(dayobject)" :class="isCheck(dayobject)?'date-active':''">{{ dayobject.day.getDate() }}</span>
                </span>
            </div>
        </div>
    </div>
    <div class="chair-warp">
        <div class="chair-row" v-for="(it,i) in (other_activity_calenddar_cu_list||[])" v-if="i<2">
            <div class="c-l-btn">
                <span>{{it.activityColumnName.slice(0,2)}}</span>
                <button>{{it.activityColumnName.slice(2,4)}}</button>
            </div>
            <div class="c-l-msg" @click="other_activity_calenddar_openurl(it.detailLink)">
                <span class="chair-title" :title="it.title">{{it.title||'暂无'}} </span>
                <span class="chair-msg">主办方：{{it.sponsor}}；活动地点：{{it.site}}；活动时间：{{(it.activityStartTime||'').slice(0,10)}}至{{(it.activityEndTime||'').slice(0,10)}}</span>
            </div>
        </div>
        <div class="temp-loading" v-if="request_of"></div><!--加载中-->
        <div class="web-empty-data" v-if="!request_of && other_activity_calenddar_cu_list && (other_activity_calenddar_cu_list||[]).length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
        </div><!--暂无数据-->
    </div>
</div>
</div>`;


    var list = document.getElementsByClassName('other_activity_calendar');
    for (var i = 0; i < list.length; i++) {
        if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
            list[i].setAttribute('class', 'other_activity_calendar jl_vip_zt_vray jl_vip_zt_warp');
            new Vue({
                el: '#' + list[i].lastChild.id,
                template: other_activity_calendar_html,
                data() {
                    return {
                        request_of:true,//请求中
                        other_activity_calendar_currentDay: 1,
                        other_activity_calendar_currentMonth: 1,
                        other_activity_calendar_currentYear: 1970,
                        other_activity_calendar_currentWeek: 1,
                        other_activity_calendar_days: [],
                        other_activity_calendar_count_list: [],//总数据
                        other_activity_calenddar_cu_days: '',//当前天
                        other_activity_calenddar_cu_list: [],//当前列表数据
                        fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
                        post_url_vip: window.apiDomainAndPort,
                        other_activity_calenddar_more_url: '#',//更多地址跳转
                    }
                },
                mounted() {
                    var now = new Date();
                    this.other_activity_calenddar_cu_days = this.formatDate(now.getFullYear(), now.getMonth() + 1, now.getDate());//默认今天
                    this.other_activity_calendar_initData(null);
                    this.other_activity_calendar_initDataList();
                },
                methods: {
                    //跳转更多详情
                    other_activity_calenddar_openurl(url) {
                        window.open(url)
                    },
                    other_activity_calendar_initDataList() {
                        var post_url = this.post_url_vip + '/activity/api/activity/all-activities?PageSize=100&PageIndex=1';
                        axios({
                            url: post_url,
                            method: 'GET',
                            headers: {
                                'Content-Type': 'text/plain',
                                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
                            },
                        }).then(res => {
                            this.request_of = false;
                            if (res.data && res.data.statusCode == 200) {
                                this.other_activity_calenddar_more_url = res.data.data.moreLink || '#';
                                this.other_activity_calendar_count_list = res.data.data.items || [];
                                this.other_dayClick({ day: new Date() });
                            }
                        }).catch(err => {
                            this.request_of = false;
                            console.log(err);
                        });
                    },
                    other_activity_calendar_initData: function (cur) {
                        var leftcount = 0; //存放剩余数量
                        var date;
                        if (cur) {
                            date = new Date(cur);
                        } else {
                            var now = new Date();
                            var d = new Date(this.formatDate(now.getFullYear(), now.getMonth() + 1, 1));
                            d.setDate(35);
                            date = new Date(this.formatDate(d.getFullYear(), d.getMonth(), 1));
                        }
                        this.other_activity_calendar_currentDay = date.getDate();
                        this.other_activity_calendar_currentYear = date.getFullYear();
                        this.other_activity_calendar_currentMonth = date.getMonth() + 1;

                        this.other_activity_calendar_currentWeek = date.getDay(); // 1...6,0
                        if (this.other_activity_calendar_currentWeek == 0) {
                            this.other_activity_calendar_currentWeek = 7;
                        }
                        var str = this.formatDate(this.other_activity_calendar_currentYear, this.other_activity_calendar_currentMonth, this.other_activity_calendar_currentDay);
                        this.other_activity_calendar_days.length = 0;
                        // 今天是周日，放在第一行第7个位置，前面6个
                        //初始化本周
                        for (var i = this.other_activity_calendar_currentWeek - 1; i >= 0; i--) {
                            var d = new Date(str);
                            d.setDate(d.getDate() - i);
                            var dayobject = {}; //用一个对象包装Date对象  以便为以后预定功能添加属性
                            dayobject.day = d;
                            this.other_activity_calendar_days.push(dayobject);//将日期放入data 中的days数组 供页面渲染使用


                        }
                        //其他周
                        for (var i = 1; i <= 35 - this.other_activity_calendar_currentWeek; i++) {
                            var d = new Date(str);
                            d.setDate(d.getDate() + i);
                            var dayobject = {};
                            dayobject.day = d;
                            this.other_activity_calendar_days.push(dayobject);
                        }
                    },
                    pickPre: function (year, month) {
                        // setDate(0); 上月最后一天
                        // setDate(-1); 上月倒数第二天
                        // setDate(dx) 参数dx为 上月最后一天的前后dx天
                        var d = new Date(this.formatDate(year, month, 1));
                        d.setDate(0);
                        this.other_activity_calendar_initData(this.formatDate(d.getFullYear(), d.getMonth() + 1, 1));
                    },
                    pickNext: function (year, month) {
                        var d = new Date(this.formatDate(year, month, 1));
                        d.setDate(35);
                        this.other_activity_calendar_initData(this.formatDate(d.getFullYear(), d.getMonth() + 1, 1));
                    },
                    pickYear: function (year, month) {
                        alert(year + "," + month);
                    },
                    // 返回 类似 2016-01-02 格式的字符串
                    formatDate: function (year, month, day) {
                        var y = year;
                        var m = month;
                        if (m < 10) m = "0" + m;
                        var d = day;
                        if (d < 10) d = "0" + d;
                        return y + "-" + m + "-" + d
                    },
                    //点击日期-默认点击当天
                    other_dayClick(val) {
                        console.log(val);
                        this.other_activity_calenddar_cu_list = [];
                        var now = val.day;
                        this.other_activity_calenddar_cu_days = this.formatDate(now.getFullYear(), now.getMonth() + 1, now.getDate());
                        this.other_activity_calendar_count_list.forEach(item => {
                            var start_time = new Date(item.activityStartTime.slice(0, 10).replace(/\-/g, "/")).getTime();
                            var end_time = new Date(item.activityEndTime.slice(0, 10).replace(/\-/g, "/")).getTime();
                            var cu_time = new Date(this.other_activity_calenddar_cu_days.replace(/\-/g, "/")).getTime();
                            // if(new Date(data1).getDate() == new Date(this.other_activity_calenddar_cu_days).getDate() && new Date(data1).getMonth() == new Date(this.other_activity_calenddar_cu_days).getMonth()&& new Date(data1).getFullYear() == new Date(this.other_activity_calenddar_cu_days).getFullYear()){
                            if ((cu_time > start_time || cu_time == start_time) && (cu_time < end_time || cu_time == end_time)) {
                                this.other_activity_calenddar_cu_list.push(item);
                            }
                        })
                        this.$forceUpdate();
                    },
                    //选中当天
                    isCheck(val) {
                        //   var is_cu = val.day.getFullYear() == new Date().getFullYear() && val.day.getMonth() == new Date().getMonth() && val.day.getDate() == new Date().getDate();
                        //   return is_cu;
                        return this.other_activity_calenddar_cu_days == this.formatDate(val.day.getFullYear(), val.day.getMonth() + 1, val.day.getDate());
                    },
                },
            });
        }
    }
}
other_activity_calendar()
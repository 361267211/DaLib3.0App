function cqu_activity_calendar_temp1() {
  var template_html = `<div class="index-warp-box"><div class="index-active-box">
  <div class="index-active-title">
    <div>
      <span class="index-active-title-red">活动日历</span>
      <span></span>
    </div>
    <span class="index-active-title-more" @click="index_activity_calenddar_openurl(index_activity_calenddar_more_url)">更多+</span>
  </div>
  <div class="index-active-cont">
    <div class="index-active-date-warp">
      <div class="index-active-date-title">{{index_activity_calendar_currentYear}}年{{index_activity_calendar_currentMonth}}月
        <span class="index-active-date-left" @click="pickPre(index_activity_calendar_currentYear,index_activity_calendar_currentMonth)"></span>
        <span class="index-active-date-right" @click="pickNext(index_activity_calendar_currentYear,index_activity_calendar_currentMonth)"></span>
      </div>
      <div class="index-active-date-list">
        <div class="active-date-list-title active-date-list-c-l">
          <span class="active-date-list-col">一</span>
          <span class="active-date-list-col">二</span>
          <span class="active-date-list-col">三</span>
          <span class="active-date-list-col">四</span>
          <span class="active-date-list-col">五</span>
          <span class="active-date-list-col">六</span>
          <span class="active-date-list-col">日</span>
        </div>
        <div class="active-date-list-data active-date-list-c-l">
          <span class="active-date-list-col" :data-day="new Date(dayobject.day).toLocaleString().slice(0,10)" v-for="(dayobject,i) in index_activity_calendar_days" :key="i" :class="dayobject.day.getMonth()+1 != index_activity_calendar_currentMonth?'other-month':''" @click="index_dayClick(dayobject)">
            <span :class="isCheck(dayobject)?'date-active':''">{{ dayobject.day.getDate() }}</span>
          </span>
        </div>
      </div>
    </div>
    <div class="index-active-txt-box" v-if="index_activity_calenddar_cu_list.length>0">
      <h5 @click="index_activity_calenddar_openurl(index_activity_calenddar_cu_list[0].detailLink)">{{index_activity_calenddar_cu_list[0].activityColumnName}}</h5>
      <div>{{index_activity_calenddar_cu_list[0].title}}</div>
      <span @click="index_activity_calenddar_openurl(index_activity_calenddar_cu_list[0].detailLink)" v-show="isBtnShow(index_activity_calenddar_cu_list[0])">报名</span>
    </div>
    <div class="index-no-data" v-else>暂无活动</div>
  </div>
</div></div>`;

  var list = document.getElementsByClassName('cqu_activity_calendar_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_activity_calendar_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      var template_temp_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        template_temp_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }

      new Vue({
        el: '#' + list[i].lastChild.id,
        template: template_html,
        data() {
          return {
            index_activity_calendar_currentDay: 1,
            index_activity_calendar_currentMonth: 1,
            index_activity_calendar_currentYear: 1970,
            index_activity_calendar_currentWeek: 1,
            index_activity_calendar_days: [],
            index_activity_calendar_count_list: [],//总数据
            index_activity_calenddar_cu_days: '',//当天
            index_activity_calenddar_cu_list: [],//当前列表数据
            post_url_vip: window.apiDomainAndPort,
            index_activity_calenddar_more_url: '#',//更多地址跳转
          }
        },
        mounted() {
          var now = new Date();
          this.index_activity_calenddar_cu_days = this.formatDate(now.getFullYear(), now.getMonth() + 1, now.getDate());//默认今天
          this.index_activity_calendar_initData(null);
          this.index_activity_calendar_initDataList();
        },
        methods: {
          //跳转更多详情
          index_activity_calenddar_openurl(url) {
            window.open(url);
          },
          index_activity_calendar_initDataList() {
            var post_url = this.post_url_vip + '/activity/api/activity/all-activities?PageSize=100&PageIndex=1';
            axios({
              url: post_url,
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.index_activity_calenddar_more_url = res.data.data.moreLink || '#';
                this.index_activity_calendar_count_list = res.data.data.items || [];
                this.index_dayClick({ day: new Date() });
              }
            }).catch(err => {
              console.log(err);
            });
          },
          index_activity_calendar_initData: function (cur) {
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
            this.index_activity_calendar_currentDay = date.getDate();
            this.index_activity_calendar_currentYear = date.getFullYear();
            this.index_activity_calendar_currentMonth = date.getMonth() + 1;

            this.index_activity_calendar_currentWeek = date.getDay(); // 1...6,0
            if (this.index_activity_calendar_currentWeek == 0) {
              this.index_activity_calendar_currentWeek = 7;
            }
            var str = this.formatDate(this.index_activity_calendar_currentYear, this.index_activity_calendar_currentMonth, this.index_activity_calendar_currentDay);
            this.index_activity_calendar_days.length = 0;
            // 今天是周日，放在第一行第7个位置，前面6个
            //初始化本周
            for (var i = this.index_activity_calendar_currentWeek - 1; i >= 0; i--) {
              var d = new Date(str);
              d.setDate(d.getDate() - i);
              var dayobject = {}; //用一个对象包装Date对象  以便为以后预定功能添加属性
              dayobject.day = d;
              this.index_activity_calendar_days.push(dayobject);//将日期放入data 中的days数组 供页面渲染使用


            }
            //其他周
            for (var i = 1; i <= 35 - this.index_activity_calendar_currentWeek; i++) {
              var d = new Date(str);
              d.setDate(d.getDate() + i);
              var dayobject = {};
              dayobject.day = d;
              this.index_activity_calendar_days.push(dayobject);
            }
          },
          pickPre: function (year, month) {
            // setDate(0); 上月最后一天
            // setDate(-1); 上月倒数第二天
            // setDate(dx) 参数dx为 上月最后一天的前后dx天
            var d = new Date(this.formatDate(year, month, 1));
            d.setDate(0);
            this.index_activity_calendar_initData(this.formatDate(d.getFullYear(), d.getMonth() + 1, 1));
          },
          pickNext: function (year, month) {
            var d = new Date(this.formatDate(year, month, 1));
            d.setDate(35);
            this.index_activity_calendar_initData(this.formatDate(d.getFullYear(), d.getMonth() + 1, 1));
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
          //是否显示报名按钮
          isBtnShow(val){
            console.log(val);
            var end_time_active = '';
            var cu_time_active = new Date(this.index_activity_calenddar_cu_days.replace(/\-/g, "/")).getTime();
            if(val.signSetEndTime){
              end_time_active = new Date(val.signSetEndTime.slice(0, 10).replace(/\-/g, "/")).getTime();
            }else{
              end_time_active = new Date(val.activityEndTime.slice(0, 10).replace(/\-/g, "/")).getTime();
            }
            if(end_time_active>cu_time_active){
              return true;
            }else{
              return false;
            }
          },
          //点击日期-默认点击当天
          index_dayClick(val) {
            console.log(val);
            this.index_activity_calenddar_cu_list = [];
            var now = val.day;
            this.index_activity_calenddar_cu_days = this.formatDate(now.getFullYear(), now.getMonth() + 1, now.getDate());
            this.index_activity_calendar_count_list.forEach(item => {
              var start_time = new Date(item.activityStartTime.slice(0, 10).replace(/\-/g, "/")).getTime();
              var end_time = new Date(item.activityEndTime.slice(0, 10).replace(/\-/g, "/")).getTime();
              var cu_time = new Date(this.index_activity_calenddar_cu_days.replace(/\-/g, "/")).getTime();
              // if(new Date(data1).getDate() == new Date(this.index_activity_calenddar_cu_days).getDate() && new Date(data1).getMonth() == new Date(this.index_activity_calenddar_cu_days).getMonth()&& new Date(data1).getFullYear() == new Date(this.index_activity_calenddar_cu_days).getFullYear()){
              if ((cu_time > start_time || cu_time == start_time) && (cu_time < end_time || cu_time == end_time)) {
                this.index_activity_calenddar_cu_list.push(item);
              }
            })
            this.$forceUpdate();
          },
          //选中当天
          isCheck(val) {
            //   var is_cu = val.day.getFullYear() == new Date().getFullYear() && val.day.getMonth() == new Date().getMonth() && val.day.getDate() == new Date().getDate();
            //   return is_cu;
            return this.index_activity_calenddar_cu_days == this.formatDate(val.day.getFullYear(), val.day.getMonth() + 1, val.day.getDate());
          },
        },
      });
    }
  }
}
cqu_activity_calendar_temp1()
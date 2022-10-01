function other_lib_activity() {
  var college_library_html = `<div class="tmp-detail">
  <div class="title-box">
    <span class="left">图书馆活动</span>
    <span class="right">
      <span @click="other_lib_activity_openurlDetails(activeMoreUrl)">更多 </span>
    </span>
  </div>
  <div class="tem-content">
    <div class="activity-item-box" v-if="!request_of">
      <div class="activity-item" :class="other_lib_activity_show_index==index?'lib-activity-item-show':''" v-for="(item,index) in other_lib_activity_list1" @click="other_lib_activity_openurlDetails(item.detailLink)"  @mouseover="clearSwiper()" @mouseout="initSwiper()">
        <img :src="item.cover" alt="">
        <div>
          <span class="act-tag">{{item.activityColumnName}}</span>
          <div>
            <span class="act-name">{{item.title}}</span>
            <span class="act-text">{{item.activityStartTime}} {{item.site}}</span>
          </div>
        </div>
      </div>
    </div>
    <!--加载中-->
    <div class="temp-loading" v-if="request_of"></div>
    <!--暂无数据-->
    <div class="web-empty-data"  v-else-if="other_lib_activity_list1.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
    </div>
  </div>
</div>`;


  var list = document.getElementsByClassName('other_lib_activity');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'other_lib_activity jl_vip_zt_vray jl_vip_zt_warp');
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: college_library_html,
        data() {
          return {
            request_of: true,
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
            other_lib_activity_list1: [],
            activeMoreUrl:'',
            other_lib_activity_show_index: 0,
            timer: null
          }
        },
        mounted() {
          this.other_lib_activity_initData();
        },
        methods: {
          //查看详情，到详情页面
          other_lib_activity_openurlDetails(url) {
            window.open(url)
          },
          other_lib_activity_initData() {
            var post_url1 = this.post_url_vip + '/activity/api/activity/my-joined-activities?PageSize=100&PageIndex=1';
            axios({
              url: post_url1,
              method: 'GET',
              headers: {
                'Content-Type': 'text/plain',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.other_lib_activity_list1 = res.data.data.items || [];
                this.getAll();
              } else {
                this.getAll();
              }
            }).catch(err => {
              this.getAll();
              console.log(err);
            });
          },
          getAll() {
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
                this.activeMoreUrl = res.data.data.moreLink;
                this.other_lib_activity_list1 = [...this.other_lib_activity_list1, ...res.data.data.items] || [];
                this.initSwiper();
              }
              this.request_of = false;
            }).catch(err => {
              this.request_of = false;
              console.log(err);
            });
          },
          //   初始化轮播图
          initSwiper() {
            if (this.other_lib_activity_list1.length > 0) {
              this.timer = setInterval(() => {
                this.other_lib_activity_show_index = this.other_lib_activity_show_index >= (this.other_lib_activity_list1.length - 1) ? 0 : this.other_lib_activity_show_index + 1;
                // console.log(this.other_lib_activity_show_index);
              }, 5000);
            }
          },
          // 清除定时器
          clearSwiper() {
            clearInterval(this.timer);
          },
        },
      });
    }
  }
}
other_lib_activity()
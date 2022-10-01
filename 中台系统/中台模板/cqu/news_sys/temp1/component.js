function cqu_news_sys_temp1() {
  var template_html = `<div class="index-warp-box"><div class="index-swiper-box">
  <div class="index-swiper-cont" @click="cqu_linkTo" v-if="!request_of">
    <div class="index-swiper-list"  @mouseover="clearSwiper()" @mouseout="initSwiper()">
      <img :src="fileUrl+'/'+item.cover" v-for="(item,index) in cqu_list.contentList" :class="showIndex==index?'index-img-show':''" :key="item.contentID" alt="">
    </div>
    <div class="index-swiper-time">
      <div :class="showIndex==index?'index-swiper-time-sel':''" @click.stop="showIndex=index" v-for="(item,index) in cqu_list.contentList" :key="item.contentID"><span>{{item.createTime.slice(5,7)}}月</span><span>{{item.createTime.slice(0,4)}}年</span></div>
    </div>
    <div class="index-swiper-title">{{cqu_list.contentList[showIndex].title}}</div>
  </div>
  <div class="index-swiper-cont" v-if="request_of||(!request_of&&cqu_list.contentList.length==0)">
    <div class="temp-loading" v-if="request_of"></div><!--加载中-->
    <div class="web-empty-data"  v-else-if="cqu_list.contentList.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
    </div><!--暂无数据-->
  </div>
</div></div>`;

  var list = document.getElementsByClassName('cqu_news_sys_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'cqu_news_sys_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      var template_temp_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        template_temp_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));

      }

      new Vue({
        el: '#' + list[i].lastChild.id,
        template: template_html,
        data() {
          return {
            imgPath: window.localStorage.getItem('fileUrl') + '/public/image/',//公共图片路径
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
            request_of: true,//请求中
            cqu_list: [],
            showIndex: 0,//显示index
            timer: null,//定时器
          }
        },
        mounted() {
          var template_temp_data_list = [];
          if (template_temp_set_list) {
            for (var i = 0; i < template_temp_set_list.length; i++) {
              var topCount = template_temp_set_list[i].topCount;
              var columnid = template_temp_set_list[i].id;
              var OrderRule = template_temp_set_list[i].sortType;
              template_temp_data_list.push({ count: topCount, columnId: columnid, sortField: OrderRule });
            }
          }
          this.cqu_initData(template_temp_data_list);
        },
        methods: {
          cqu_linkTo() {
            if (this.cqu_list.contentList[this.showIndex].externalLink && this.cqu_list.contentList[this.showIndex].externalLink !== '') {
              window.open(this.cqu_list.contentList[this.showIndex].externalLink);
            } else {
              window.open(this.cqu_list.contentList[this.showIndex].jumpLink);
            }
          },
          cqu_initData(list) {
            var post_url = this.post_url_vip + '/news/api/news/pront-scenes-news';
            axios({
              url: post_url,
              method: 'POST',
              data: list,
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              if (res.data && res.data.statusCode == 200) {
                this.cqu_list = res.data.data[0] || [];
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
            if (this.cqu_list.contentList.length > 0) {
              this.timer = setInterval(() => {
                this.showIndex = this.showIndex >= (this.cqu_list.contentList.length - 1) ? 0 : this.showIndex + 1;
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
cqu_news_sys_temp1()
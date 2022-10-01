function news_sys_temp1() {
  var news_sys_temp1_html = `<div class="news-temp-warp">
<div class="news-temp-content c-l">
    <div class="news-temp-left">
        <div class="main-title-temp"><span>{{news_sys_temp1_list1.columnName||'无'}}</span><span class="e-title"></span><span class="r-btn" @click="news_sys_temp1_openurl(news_sys_temp1_list1.jumpLink)">查看更多 ></span></div>
        <div class="news-content c-l">
            <div class="news-row" v-for="i in (news_sys_temp1_list1.contentList||[])" @click="news_sys_temp1_openurlDetails(i)"><span class="title">{{i.title}}</span><span class="date-time">{{(i.publishDate||'').slice(0,10)}}</span></div>
        </div>
        <div class="temp-loading" v-if="request_of"></div><!--加载中-->
        <div class="web-empty-data" v-if="!request_of && news_sys_temp1_list1 && (news_sys_temp1_list1.contentList||[]).length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
        </div><!--暂无数据-->
    </div>
    <div class="news-temp-right">
        <div class="main-title-temp"><span>{{news_sys_temp1_list2.columnName||'无'}}</span><span class="e-title"></span><span class="r-btn" @click="news_sys_temp1_openurl(news_sys_temp1_list2.jumpLink)">查看更多 ></span></div>
        <div class="news-content c-l">
            <div class="news-row" v-for="i in (news_sys_temp1_list2.contentList||[])" @click="news_sys_temp1_openurlDetails(i)"><span class="title">{{i.title}}</span><span class="date-time">{{(i.publishDate||'').slice(0,10)}}</span></div>
        </div>
        <div class="temp-loading" v-if="request_of"></div><!--加载中-->
        <div class="web-empty-data" v-if="!request_of && news_sys_temp1_list2 && (news_sys_temp1_list2.contentList||[]).length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
        </div><!--暂无数据-->
    </div>
</div>
</div>`;
  //jl_vip_zt_vray 渲染过一次就不再重写渲染，以此为标记;
  //jl_vip_zt_warp 必须添加，是为了渲染里面的删除标签等样式

  var list = document.getElementsByClassName('news_sys_temp1');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'news_sys_temp1 jl_vip_zt_vray jl_vip_zt_warp');
      var news_sys_temp1_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        news_sys_temp1_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: news_sys_temp1_html,
        data() {
          return {
            request_of:true,//请求中
            news_sys_temp1_list1: {},
            news_sys_temp1_list2: {},
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
            news_sys_temp1_more_url: '#',//更多地址跳转
          }
        },
        mounted() {
          if (news_sys_temp1_set_list) {
            var list = [];
            for (var i = 0; i < news_sys_temp1_set_list.length; i++) {
              var topCount = news_sys_temp1_set_list[i].topCount;
              var columnid = news_sys_temp1_set_list[i].id;
              var OrderRule = news_sys_temp1_set_list[i].sortType;
              list.push({ count: topCount, columnId: columnid, sortField: OrderRule });
            }
            this.news_sys_temp1_initData(list);
          }
        },
        methods: {
          //查看更多，到列表页面
          news_sys_temp1_openurl(url, c_id) {
            window.open(url);
          },
          //查看详情，到详情页面
          news_sys_temp1_openurlDetails(val) {
            if (val.externalLink && val.externalLink != '' && val.externalLink != undefined) {
              window.open(val.externalLink)
            } else {
              window.open(val.jumpLink)
            }
          },
          news_sys_temp1_initData(list) {
            var post_url = this.post_url_vip + '/news/api/news/pront-scenes-news';
            axios({
              url: post_url,
              method: 'post',
              data: list,
              headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
              },
            }).then(res => {
              this.request_of = false;
              if (res.data && res.data.statusCode == 200) {
                if (res.data.data.length > 0) {
                  this.news_sys_temp1_list1 = res.data.data[0] || {};
                  if (res.data.data.length > 1) {
                    this.news_sys_temp1_list2 = res.data.data[1] || {};
                  }
                }
              }
            }).catch(err => {
              this.request_of = false;
              console.log(err);
            });
          },
        },
      });
    } else {

    }
  }
}
news_sys_temp1()


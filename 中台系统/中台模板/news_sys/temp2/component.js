function news_sys_temp2() {
  var news_sys_temp2_html = `<div class="tmp-detail">
  <div class="title-box">
    <span class="left">{{news_sys_temp2_list.columnName || '图书馆新闻'}}</span>
    <span class="right">
      <span @click="linkTo(news_sys_temp2_list.jumpLink)">更多 </span>
    </span>
  </div>
  <div class="tem-content">
    <p class="news-item" v-for="item in news_sys_temp2_list.contentList" :key="item" @click="news_sys_temp2_openurlDetails(item.jumpLink,news_sys_temp2_list.columnID,item.contentID,item.externalLink)" v-if="!request_of">
      <span class="nowarp">
        <span class="tag bule">{{item.lables}}</span>
        {{item.title}}
      </span>
      <span>{{(item.publishDate||'').slice(0,10)}}</span>
    </p>
    <!--加载中-->
    <div class="temp-loading" v-if="request_of"></div>
    <!--暂无数据-->
    <div class="web-empty-data"  v-else-if="news_sys_temp2_list.contentList.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
    </div>
  </div>
</div>`;
  //jl_vip_zt_vray 渲染过一次就不再重写渲染，以此为标记;
  //jl_vip_zt_warp 必须添加，是为了渲染里面的删除标签等样式

  var list = document.getElementsByClassName('news_sys_temp2');
  for (var i = 0; i < list.length; i++) {
    if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
      list[i].setAttribute('class', 'news_sys_temp2 jl_vip_zt_vray jl_vip_zt_warp');
      var news_sys_temp2_set_list = null;
      if (list[i].dataset && list[i].dataset.set) {
        news_sys_temp2_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
      }
      new Vue({
        el: '#' + list[i].lastChild.id,
        template: news_sys_temp2_html,
        data() {
          return {
            request_of: true,
            fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
            post_url_vip: window.apiDomainAndPort,
            news_sys_temp2_list: {},
          }
        },
        mounted() {
          if (news_sys_temp2_set_list) {
            for (var i = 0; i < news_sys_temp2_set_list.length; i++) {
              var topCount = news_sys_temp2_set_list[i].topCount;
              var columnid = news_sys_temp2_set_list[i].id;
              var OrderRule = news_sys_temp2_set_list[i].sortType;
              var news_sys_temp2_data_list = [{ count: topCount, columnId: columnid, sortField: OrderRule }];
              this.news_sys_temp2_initData(news_sys_temp2_data_list);
            }
          }
        },
        methods: {
          linkTo(url){
            window.open(url)
          },
          //查看详情，到详情页面
          news_sys_temp2_openurlDetails(url, c_id, id, externalLink) {
            if (externalLink && externalLink != '' && externalLink != undefined) {
              window.open(externalLink)
              return;
            }
            window.open(url)
          },
          news_sys_temp2_initData(list) {
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
                this.news_sys_temp2_list = res.data.data[0];
              }
              this.request_of = false;
            }).catch(err => {
              this.request_of = false;
              console.log(err);
            });
          },
        },
      });
    }
  }
}
news_sys_temp2()


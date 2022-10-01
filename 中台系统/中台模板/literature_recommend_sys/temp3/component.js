function literature_recommend_sys_temp3() {
    var literature_recommend_sys_temp3_html = `<div class="tmp-detail">
    <div class="title-box">
    <span class="left">{{literature_recommend_sys_temp3_allList_fist.name||'资源推荐类'}}</span>
    <span class="right">
        <span @click="handleChange">换一换</span>
    </span>
    </div>
    <div class="tem-content">
        <div class="resource-item" v-if="!request_of" v-for="item in literature_recommend_sys_temp3_allList_fist.titleInfos" :key="item" @click="literature_recommend_sys_temp3_openurlDetails(item.detail_url)">
            <img :src="item.cover" alt="">
            <p>{{item.title}}</p>
        </div>
        <!--加载中-->
        <div class="temp-loading" v-if="request_of"></div>
        <!--暂无数据-->
        <div class="web-empty-data"  v-else-if="literature_recommend_sys_temp3_allList_fist.titleInfos.length==0" :style="{background: 'url('+fileUrl+'/public/image/data-empty.png) no-repeat center'}" >
        </div>
    </div>
</div>`;

    var list = document.getElementsByClassName('literature_recommend_sys_temp3');
    for (var i = 0; i < list.length; i++) {
        if (list[i].getAttribute('class').indexOf('jl_vip_zt_vray') < 0) {
            list[i].setAttribute('class', 'literature_recommend_sys_temp3 jl_vip_zt_vray jl_vip_zt_warp');
            var literature_recommend_sys_temp3_set_list = null;
            if (list[i].dataset && list[i].dataset.set) {
                literature_recommend_sys_temp3_set_list = JSON.parse(list[i].dataset.set.replace(/'/g, '"'));
            }
            new Vue({
                el: '#' + list[i].lastChild.id,
                template: literature_recommend_sys_temp3_html,
                data() {
                    return {
                        request_of: true,
                        fileUrl: window.localStorage.getItem('fileUrl'),//图片地址前缀
                        post_url_vip: window.apiDomainAndPort,
                        literature_recommend_sys_temp3_allList_fist: {},
                        pageIndex: 1
                    }
                },
                mounted() {
                    // this.literature_recommend_sys_temp3_initData();
                    if (literature_recommend_sys_temp3_set_list && literature_recommend_sys_temp3_set_list.length > 0) {
                        for (var i = 0; i < literature_recommend_sys_temp3_set_list.length; i++) {
                            var set_content = literature_recommend_sys_temp3_set_list[i];
                            var topCount = '';
                            var columnid = '';
                            var OrderRule = '';
                            if (set_content) {
                                topCount = set_content['topCount'];
                                columnid = set_content['id'];
                                OrderRule = set_content['sortType'];
                            }
                            this.literature_recommend_sys_temp3_initData(topCount, columnid, OrderRule);
                        }
                    }
                },
                methods: {
                    //查看详情，到详情页面
                    literature_recommend_sys_temp3_openurlDetails(url) {
                        window.open(url);
                    },
                    // 换一换
                    handleChange() {
                        this.pageIndex++;
                        if (literature_recommend_sys_temp3_set_list && literature_recommend_sys_temp3_set_list.length > 0) {
                            for (var i = 0; i < literature_recommend_sys_temp3_set_list.length; i++) {
                                var set_content = literature_recommend_sys_temp3_set_list[i];
                                var topCount = '';
                                var columnid = '';
                                var OrderRule = '';
                                if (set_content) {
                                    topCount = set_content['topCount'];
                                    columnid = set_content['id'];
                                    OrderRule = set_content['sortType'];
                                }
                                this.literature_recommend_sys_temp3_initData(topCount, columnid, OrderRule);
                            }
                        }
                    },
                    literature_recommend_sys_temp3_initData(topCount, columnid, OrderRule) {
                        var post_url = this.post_url_vip + '/articlerecommend/api/sceneuse/getsceneusebyid';
                        if (topCount) {
                            post_url = post_url + '?Count=' + topCount;
                        }
                        if (columnid) {
                            post_url = post_url + '&ColumnId=' + columnid;
                        }
                        if (OrderRule) {
                            post_url = post_url + '&OrderRule=' + OrderRule;
                        }
                        post_url = post_url + '&pageIndex=' + this.pageIndex;
                        axios({
                            url: post_url,
                            method: 'GET',
                            headers: {
                                'Content-Type': 'text/plain',
                                'Authorization': 'Bearer ' + window.localStorage.getItem('token'),
                            },
                        }).then(res => {
                            if (res.data && res.data.statusCode == 200) {
                                this.literature_recommend_sys_temp3_allList_fist = res.data.data;
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
literature_recommend_sys_temp3()
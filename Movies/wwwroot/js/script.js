const _actions = {};

function request(options) {
    const query = options.query ?? {};
    const body = options.method === 'GET' ? null : JSON.stringify(options.body);
    query["suppress_status_code"] = '1';
    let headers = options.headers ?? {};
    headers[Core.csrf.header] = Core.csrf.value();
    const contentType = 'application/json';
    $.ajax({
        type: options.method,
        url: formatUrl(options.path, query),
        contentType: contentType,
        cache: false,
        headers: headers,
        data: body,
        processData: false,
        success: function (response) {
            options.onSuccess(response);
        }
    })
}

function formatUrl(path, params) {
    let query = '', index = 0;
    if (params) {
        Object.keys(params).forEach(x => {
            query += (index == 0 ? '?' : '&') + encodeURI(x) + '=' + encodeURI(params[x]);
            index++;
        });
    }
    return path + query;
}

const Core = {
    actions: {
        registerAction: function (name, action) {
            _actions[name] = action;
        },
        fire: function (name, source, e) {
            if (name) {
                const action = _actions[name];
                if (action) {
                    e && e.preventDefault();
                    action(source);
                    return true;
                }
            }
            return false;
        }
    },
    csrf: {
        field: "__csrfToken",
        header: "__csrfToken",
        value: function () {
            return $(`input[name=${this.field}]`).val().toString();
        }
    },
    safeNavigate: function (link, writeToHistory, e) {
        if (link === '#') {
            e && e.preventDefault();
            return;
        }

        if (!link.match('#') && !link.match('javascript')) {
            e && e.preventDefault();

            console.log('navigating ' + link);
            $('#page-reloadable').load(link, function () {
                if ($('#page_title').length)
                    document.title = $('#page_title').text();

                if (writeToHistory)
                    history.pushState('data', document.title, link);
            });
        }
    },
    get: function (path, data, options) {
        const queryData = data ?? {};
        request({
            ...options,
            method: 'GET',
            path: path,
            query: {
                ...options.query,
                ...queryData
            }
        });
    },
    post: function (path, body, options) {
        request({
            ...options,
            body: body,
            method: 'POST',
            path: path
        });
    }
}

$(document).on("click", "button", function (e) {
    Core.actions.fire($(this).data('action'), $(this), e);
});
$(document).on("click", "a", function (e) {
    const actionFired = Core.actions.fire($(this).data('action'), $(this), e);
    if ($(this).attr('target') != '_blank' && $(this).data("link-regular") === undefined) {
        if (!actionFired) {
            const link = $(this).attr('href');
            Core.safeNavigate(link, true, e);
        }
    }
});


Core.actions.registerAction("movies.loadMore", loadMoreMovies);
Core.actions.registerAction("favorites.remove", removeFromFavorites);
Core.actions.registerAction("favorites.add", addToFavorites);

function loadMoreMovies(source) {
    $(source).text('Loading..');

    const action = $(source).data('action');
    const endpoint = $(source).data('movies');
    const page = $(source).data('next-page');

    Core.get(endpoint, {
        page: page  
    }, {
        headers: {
            pagination: true
        },
        onSuccess: function (result) {
            $(source).text('Load more');
            const loadMoreButton = $('div[name=movies]').append(result).find(`[data-action="${action}"]`);
            if (loadMoreButton)
                $(source).parent().html(loadMoreButton);
            else $(source).parent().remove();
        }
    })
}

function addToFavorites(source) {
    const movie = $(source).data('id');
    Core.post("/favorites", {
        movieId: movie
    }, {
        onSuccess: function () {
            $('[data-action="favorites.add"][data-id=' + movie + "]").addClass("d-none");
            $('[data-action="favorites.remove"][data-id=' + movie + "]").removeClass("d-none");
        }
    });
}
function removeFromFavorites(source) {
    const movie = $(source).data('id');
    request({
        method: 'DELETE',
        path: '/favorites',
        body: {
            movieId: movie
        },
        onSuccess: function () {
            $('[data-action="favorites.remove"][data-id=' + movie + "]").addClass("d-none");
            $('[data-action="favorites.add"][data-id=' + movie + "]").removeClass("d-none");
        }
    })
}